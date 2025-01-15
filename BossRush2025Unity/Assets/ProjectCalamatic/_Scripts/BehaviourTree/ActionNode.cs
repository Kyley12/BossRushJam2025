using UnityEngine;

[CreateAssetMenu(fileName = "ActionNode", menuName = "Scriptable Objects/ActionNode")]
public class ActionNode : TreeNode
{
    public string actionName;

    public override bool Execute(BossAI bossAI)
    {
        switch (actionName)
        {
            case "MoveRandomly":
                return MoveRandomly(bossAI);

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
                // End the halt
                isHalted = false;
                haltTimer = 0f;
                Debug.Log("Boss resumes movement.");
            }
            else
            {
                return false; // Still halted, no movement
            }
        }

        // Choose a new random target position if none is set or the target is reached
        if (targetPosition == Vector3.zero || Vector3.Distance(bossAI.transform.position, targetPosition) < 0.1f)
        {
            // Get camera bounds
            Camera mainCamera = Camera.main;
            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

            // Generate random position within bounds
            float randomX = Random.Range(bottomLeft.x, topRight.x);
            float randomY = Random.Range(Mathf.Max(bottomLeft.y, 0), topRight.y); // Ensure Y >= 0

            targetPosition = new Vector3(randomX, Mathf.Max(randomY, 0), bossAI.transform.position.z); // Y offset limit applied

            // Randomly decide to halt after reaching this target
            if (Random.value < 0.3f) // 30% chance to halt after reaching the target
            {
                haltDuration = Random.Range(1f, 3f); // Halt for 1-3 seconds
                isHalted = true;
                Debug.Log($"Boss halts for {haltDuration} seconds.");
            }
        }

        // Move the boss toward the target position
        float moveSpeed = 3f; // Example move speed
        bossAI.transform.position = Vector3.MoveTowards(
            bossAI.transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Clamp the boss's Y position to ensure it stays above the offset limit
        bossAI.transform.position = new Vector3(
            bossAI.transform.position.x,
            Mathf.Max(bossAI.transform.position.y, 0), // Ensure Y >= 0
            bossAI.transform.position.z
        );

        return Vector3.Distance(bossAI.transform.position, targetPosition) < 0.1f; // Return true when close to the target
    }
}
