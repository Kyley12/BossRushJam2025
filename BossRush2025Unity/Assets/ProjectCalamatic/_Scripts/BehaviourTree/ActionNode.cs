using UnityEngine;

[CreateAssetMenu(fileName = "ActionNode", menuName = "Scriptable Objects/ActionNode")]
public class ActionNode : TreeNode
{
    public string actionName;

    private BulletType currentBulletType; // The currently selected bullet type
    private int randomPatternIndex;
    private float fireTimer = 0f;

    public BossPatterns bossPatterns; // Holds patterns for all bullet types
    public ScriptableObject websiteBulletDataSO; // Website-specific data
    public BossStatSo bossStats; // Reference to boss stats

    public override bool Execute(BossAI bossAI)
    {

        switch (actionName)
        {
            case "MoveRandomly":
                return MoveRandomly(bossAI);
            case "FireBullet":
                return FireBullet(bossAI);

            default:
                Debug.LogWarning($"Unknown action: {actionName}");
                return false;
        }
    }

    private Vector3 targetPosition;
    private float haltTimer = 0f; // Timer to manage halts
    private bool isHalted = false; // Whether the boss is currently halted
    private float haltDuration = 0f; // Duration of the current halt

    private bool MoveRandomly(BossAI bossAI)
    {
        if (isHalted)
        {
            haltTimer += Time.deltaTime;

            if (haltTimer >= haltDuration)
            {
                isHalted = false;
                haltTimer = 0f;
            }
            else
            {
                return false;
            }
        }

        if (targetPosition == Vector3.zero || Vector3.Distance(bossAI.transform.position, targetPosition) < 0.1f)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera not found!");
                return false;
            }

            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

            float randomX = Random.Range(bottomLeft.x, topRight.x);
            float randomY = Random.Range(Mathf.Max(bottomLeft.y, 0), topRight.y);

            targetPosition = new Vector3(randomX, Mathf.Max(randomY, 0), bossAI.transform.position.z);

            if (Random.value < 0.3f)
            {
                haltDuration = Random.Range(0.1f, 1f);
                isHalted = true;
            }
        }

        float moveSpeed = 2f;
        bossAI.transform.position = Vector3.MoveTowards(
            bossAI.transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        return Vector3.Distance(bossAI.transform.position, targetPosition) < 0.1f;
    }

    private bool FireBullet(BossAI bossAI)
    {
        fireTimer += Time.deltaTime;

        BulletPattern pattern = bossPatterns.GetPattern(currentBulletType, randomPatternIndex);
        Debug.Log(pattern);
        Debug.Log(fireTimer);

        if (pattern == null)
        {
            Debug.LogError($"Pattern not found for BulletType: {currentBulletType}, PatternIndex: {randomPatternIndex}");
            return false;
        }

        if (fireTimer >= pattern.fireRate)
        {
            SelectBulletTypeAndPattern();
            FirePattern(bossAI);
            BulletPool.bulletPoolInstance.ClearPool();
            fireTimer = 0f;
            return true;
        }



        return false;
    }

    private void SelectBulletTypeAndPattern()
    {
        currentBulletType = (BulletType)Random.Range(0, 3);

        var patterns = bossPatterns.GetPatterns(currentBulletType);
        if (patterns == null || patterns.Count == 0)
        {
            return;
        }

        randomPatternIndex = Random.Range(0, patterns.Count);
    }

    private void FirePattern(BossAI bossAI)
    {
        var pattern = bossPatterns.GetPattern(currentBulletType, randomPatternIndex);

        if (pattern == null)
        {
            Debug.LogError("Failed to retrieve bullet pattern");
            return;
        }

        float angleStep = (pattern.endAngle - pattern.startAngle) / (pattern.bulletsAmount - 1);
        float angle = pattern.startAngle;

        for (int i = 0; i < pattern.bulletsAmount; i++)
        {
            float bulletDirectionX = bossAI.transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulletDirectionY = bossAI.transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, bossAI.transform.position.z);
            Vector3 bulletDirection = (bulletMoveVector - bossAI.transform.position).normalized;

            GameObject bullet = BulletPool.bulletPoolInstance.GetBullet(currentBulletType);
            if (bullet == null)
            {
                Debug.LogError("Failed to retrieve bullet from BulletPool");
                continue;
            }

            bullet.transform.position = bossAI.transform.position;
            bullet.transform.rotation = Quaternion.identity;
            bullet.SetActive(true);

            switch (currentBulletType)
            {
                case BulletType.Normal:
                    bullet.GetComponent<NormalBullet>().Initialize(pattern, bulletDirection);
                    break;
                case BulletType.Website:
                    bullet.GetComponent<WebsiteBullet>().Initialize(pattern, websiteBulletDataSO);
                    break;
                case BulletType.Deflectable:
                    bullet.GetComponent<DeflectableBullet>().Initialize(pattern, bossStats);
                    break;
                default:
                    Debug.LogWarning("Unhandled bullet type");
                    break;
            }

            angle += angleStep;
        }
    }
}

public enum BulletType
{
    Website,
    Normal,
    Deflectable
}
