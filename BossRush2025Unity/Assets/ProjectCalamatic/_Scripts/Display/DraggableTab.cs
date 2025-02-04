using UnityEngine;

public class DraggableTab : MonoBehaviour
{
    private PlayerCursorMovement playerCursor; // Reference to the player cursor
    private bool isDragging = false;
    private Vector3 offset;

    private void Start()
    {
        // Locate the PlayerCursorMovement instance dynamically if not assigned
        if (PlayerCursorMovement.Instance != null)
        {
            playerCursor = PlayerCursorMovement.Instance;
        }
        else
        {
            Debug.LogError("PlayerCursorMovement instance not found! Ensure it exists in the start scene.");
        }
    }

    private void Update()
    {
        if (playerCursor == null) return;

        Vector2 playerCursorPosition = playerCursor.GetCursorPosition();

        // Detect if the player cursor is overlapping the tab's collider
        Collider2D collider = GetComponent<Collider2D>();

        // Check if the click is on a button (child object)
        bool isButtonClicked = false;
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(playerCursorPosition);

        foreach (var hit in hitColliders)
        {
            if (hit.gameObject != gameObject && hit.GetComponent<SpriteButton>() != null)
            {
                isButtonClicked = true;
                break;
            }
        }

        if (Input.GetMouseButtonDown(0) && collider != null && collider.OverlapPoint(playerCursorPosition) && !isButtonClicked)
        {
            Debug.Log("Player Cursor Clicked on Tab");
            offset = transform.position - (Vector3)playerCursorPosition;
            isDragging = true;
        }

        if (isDragging)
        {
            if (Input.GetMouseButton(0)) // While left mouse button is held
            {
                Debug.Log("Dragging Tab with Player Cursor");
                Vector2 cursorWorldPosition = playerCursor.GetCursorPosition();
                transform.position = (Vector3)cursorWorldPosition + offset;
            }

            if (Input.GetMouseButtonUp(0)) // When left mouse button is released
            {
                Debug.Log("Released Tab");
                isDragging = false;
            }
        }
    }
}