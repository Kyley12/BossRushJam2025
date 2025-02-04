using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCursorMovement : MonoBehaviour
{
    public static PlayerCursorMovement Instance { get; private set; }

    public bool isRequiredCutsceneEnded; // Determines if WASD movement is enabled
    public float moveSpeed = 5f; // Speed for WASD movement
    public float jumpForce = 5f; // Force for jumping
    private int jumpCount = 0; // Tracks the number of jumps performed
    private int maxJumps = 2; // Maximum number of jumps allowed
    private Camera mainCamera;
    private Vector2 minBounds; // Minimum camera bounds
    private Vector2 maxBounds; // Maximum camera bounds
    private Rigidbody2D rb; // Rigidbody2D for WASD movement

    public GameObject playerHPText;

    public static bool isDeflectKeyPressed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load
    }

    private void Start()
    {
        InitializeCursor();
    }

    private void OnEnable()
    {
        // Listen to scene load events to reinitialize the camera
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from scene load events
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (isRequiredCutsceneEnded)
        {
            EnableRigidbody2D(); // Ensure Rigidbody2D is enabled in WASD mode
            HandleWASDMovement();
        }
        else
        {
            DisableRigidbody2D(); // Disable Rigidbody2D in mouse movement mode
            HandleMouseMovement();
        }
    }

    private void InitializeCursor()
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

        // Apply the correct movement mode after initialization
        if (isRequiredCutsceneEnded)
        {
            EnableRigidbody2D();
        }
        else
        {
            DisableRigidbody2D();
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
        if (rb == null) return;

        // WASD input for cursor movement
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right keys

        // Apply horizontal movement
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // Check for jump with W key
        if (Input.GetKeyDown(KeyCode.W) && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++; // Increment the jump count
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isDeflectKeyPressed = true;
        }
        // Set isDeflectKeyPressed to false when space is released
        else
        {
            isDeflectKeyPressed = false;
        }
        // Clamp the position within the camera bounds
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        clampedPosition.y = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);
        transform.position = clampedPosition;
    }

    private void EnableRigidbody2D()
    {
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1; // Set gravity scale to normal for 2D plane
            rb.freezeRotation = true; // Prevent rotation
        }
    }

    private void DisableRigidbody2D()
    {
        if (rb != null)
        {
            Destroy(rb); // Remove Rigidbody2D when not in WASD mode
            rb = null;
            ResetJumpState(); // Reset jump count when switching modes
        }
    }

    private void ResetJumpState()
    {
        jumpCount = 0; // Reset jump count when switching modes
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect collision with ground
        if (collision.contacts[0].normal.y > 0.5f)
        {
            jumpCount = 0; // Reset jump count when grounded
        }
    }

    private void CalculateCameraBounds()
    {
        // Calculate the camera's world bounds based on orthographic size and aspect ratio
        if (mainCamera == null) return;

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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeCursor(); // Reinitialize the cursor for the new scene
    }
}
