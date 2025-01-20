using UnityEngine;

public class DeflectableBullet : MonoBehaviour
{
    public BulletPattern bulletPattern; // Variables like speed, etc.
    public BossStatSo bossStats; // Reference to boss stats
    private Vector2 moveDirection; // Current move direction
    public float moveSpeed = 20f;
    public bool isDeflected = false; // Whether the bullet has been deflected

    private Camera mainCamera;

    public PlayerStatSO playerStat;

    private void Start()
    {
        mainCamera = Camera.main;
        SetInitialDirection();
    }

    private void Update()
    {
        // Move the bullet
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        if(!IsWithinCameraBounds())
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Initializes the bullet with its pattern and boss stats.
    /// </summary>
    /// <param name="pattern">Bullet pattern data.</param>
    /// <param name="bossStat">Reference to the boss stats.</param>
    public void Initialize(BulletPattern pattern, BossStatSo bossStat)
    {
        bulletPattern = pattern;
        bossStats = bossStat;
    }

    /// <summary>
    /// Sets the initial direction towards the player.
    /// </summary>
    private void SetInitialDirection()
    {
        if (PlayerCursorMovement.Instance != null)
        {
            Vector2 playerPosition = PlayerCursorMovement.Instance.GetCursorPosition();
            moveDirection = (playerPosition - (Vector2)transform.position).normalized;
        }
    }
    private bool IsWithinCameraBounds()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Bullet is deflected
            if (PlayerCursorMovement.isDeflectKeyPressed)
            {
                Debug.Log("Deflected Bullet");
                isDeflected = true;
                moveDirection = -moveDirection;
            }
            else
            {
                playerStat.DecreaseHealth(2f);
                gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.CompareTag("Boss") && isDeflected)
        {
            // Decrease the boss's stun bar
            Debug.Log("Boss got hit!");
            bossStats.bossStunbarHealth -= 20f;
            if (bossStats.bossStunbarHealth < 0)
                bossStats.bossStunbarHealth = 0;

            gameObject.SetActive(false);
            isDeflected = false; // Deactivate bullet after hitting the boss
        }
    }
}
