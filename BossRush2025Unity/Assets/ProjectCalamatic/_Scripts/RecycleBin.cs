using UnityEngine;

public class RecycleBin : MonoBehaviour
{
    private BattleHandler battleHandler;
    private Rigidbody2D rb;

    private bool hasLanded = false;

    public LayerMask groundLayer; // Assign the ground layer in the Inspector


    public void Initialize(BattleHandler handler)
    {
        battleHandler = handler;
        rb = GetComponent<Rigidbody2D>();
    }

     private void Update()
    {
        if (!hasLanded && rb != null)
        {
            // Use a raycast to check if the recycle bin has landed
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
            if (hit.collider != null)
            {
                hasLanded = true;

                // Change bodyType to Kinematic to stop physics interactions
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            battleHandler.RetrieveFolderFromRecycleBin();
            FolderManager.isReterived = true;
        }
    }
}
