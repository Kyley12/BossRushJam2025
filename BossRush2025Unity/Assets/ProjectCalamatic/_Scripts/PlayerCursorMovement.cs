using UnityEngine;

public class PlayerCursorMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to control the player's movement speed
    private Camera mainCamera;
    private const float zOffset = 0f; // Fixed Z offset for 2D environment

    private Vector2 minBounds; // Minimum bounds of the camera view
    private Vector2 maxBounds; // Maximum bounds of the camera view

    private void Start()
    {
        mainCamera = Camera.main; // Get reference to the main camera
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Ensure the Main Camera is tagged as 'MainCamera'.");
            return;
        }

        CalculateCameraBounds();

        // Hide the system cursor
        Cursor.visible = false;

        // Lock the cursor to the game window
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            // Get the mouse position in screen space
            Vector3 cursorScreenPosition = Input.mousePosition;

            // Convert screen position to world position with a fixed Z offset
            Vector3 cursorWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(cursorScreenPosition.x, cursorScreenPosition.y, -mainCamera.transform.position.z));

            // Set the Z offset for 2D environment
            cursorWorldPosition.z = zOffset;

            // Clamp the cursor's position within the camera bounds
            cursorWorldPosition.x = Mathf.Clamp(cursorWorldPosition.x, minBounds.x, maxBounds.x);
            cursorWorldPosition.y = Mathf.Clamp(cursorWorldPosition.y, minBounds.y, maxBounds.y);

            // Move the player toward the cursor's position at a slower speed
            transform.position = Vector3.Lerp(transform.position, cursorWorldPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void CalculateCameraBounds()
    {
        // Calculate the camera's world bounds based on orthographic size and aspect ratio
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        Vector3 cameraPosition = mainCamera.transform.position;

        minBounds = new Vector2(cameraPosition.x - cameraWidth / 2f, cameraPosition.y - cameraHeight / 2f);
        maxBounds = new Vector2(cameraPosition.x + cameraWidth / 2f, cameraPosition.y + cameraHeight / 2f);
    }

    private void OnDrawGizmos()
    {
        if (mainCamera == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, zOffset), new Vector3(maxBounds.x, minBounds.y, zOffset));
        Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, zOffset), new Vector3(maxBounds.x, maxBounds.y, zOffset));
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, zOffset), new Vector3(minBounds.x, maxBounds.y, zOffset));
        Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, zOffset), new Vector3(maxBounds.x, maxBounds.y, zOffset));
    }
}
