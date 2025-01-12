using UnityEngine;

public class DraggableTab : MonoBehaviour
{
    public PlayerCursorMovement playerCursor; // Reference to the player cursor
    private bool isDragging = false;
    private Vector3 offset;

    private void Start()
    {
        if (playerCursor == null)
        {
            Debug.LogError("PlayerCursor reference is missing! Assign it in the Inspector.");
        }
    }

    private void Update()
    {
        if (playerCursor == null) return;

        Vector2 playerCursorPosition = playerCursor.GetCursorPosition();

        // Detect if the player cursor is overlapping the tab's collider
        Collider2D collider = GetComponent<Collider2D>();
        if (Input.GetMouseButtonDown(0) && collider != null && collider.OverlapPoint(playerCursorPosition))
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
