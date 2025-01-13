using UnityEngine;

public class PlayerCursorMovement : MonoBehaviour
{
    public static PlayerCursorMovement Instance { get; private set; }

    public bool isRequiredCutsceneEnded;
    public float moveSpeed = 5f; // Speed for WASD movement
    private Camera mainCamera;
    private Vector2 minBounds; // Minimum camera bounds
    private Vector2 maxBounds; // Maximum camera bounds

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Ensure the Main Camera is tagged as 'MainCamera'.");
            return;
        }

        // Hide the system cursor
        Cursor.visible = false;

        // Confine the cursor to the game window
        Cursor.lockState = CursorLockMode.Confined;

        // Calculate camera bounds
        CalculateCameraBounds();
    }

    private void Update()
    {
        if (isRequiredCutsceneEnded)
        {
            HandleWASDMovement();
        }
        else
        {
            HandleMouseMovement();
        }
    }

    private void HandleMouseMovement()
    {
        if (mainCamera != null)
        {
            // Get the mouse position in screen space
            Vector3 mousePosition = Input.mousePosition;

            // Convert screen position to world position
            Vector3 cursorWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -mainCamera.transform.position.z));

            // Clamp the cursor position within the camera bounds
            cursorWorldPosition.x = Mathf.Clamp(cursorWorldPosition.x, minBounds.x, maxBounds.x);
            cursorWorldPosition.y = Mathf.Clamp(cursorWorldPosition.y, minBounds.y, maxBounds.y);

            // Set the Z position to 0 (or your desired Z value for 2D)
            cursorWorldPosition.z = 0;

            // Update the player cursor's position to exactly match the clamped world position
            transform.position = cursorWorldPosition;
        }
    }

    private void HandleWASDMovement()
    {
        // WASD input for cursor movement
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right keys
        float moveY = Input.GetAxis("Vertical");   // W/S or Up/Down keys

        // Calculate the new position
        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;

        // Clamp the position within the camera bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        // Update the cursor position
        transform.position = newPosition;
    }

    public bool IsSpaceKeyPressed()
    {
        // Check for Space key press
        return isRequiredCutsceneEnded && Input.GetKeyDown(KeyCode.Space);
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

    public Vector2 GetCursorPosition()
    {
        return transform.position;
    }
}
