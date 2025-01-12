using UnityEngine;

public class SpriteButton : MonoBehaviour
{
    public Sprite normalSprite;  // Default button sprite
    public Sprite hoverSprite;   // Sprite when hovering
    public Sprite clickedSprite; // Sprite when clicked

    public PlayerCursorMovement playerCursor; // Reference to the player cursor object
    private SpriteRenderer spriteRenderer;
    private bool isHovering = false; // Track if the player cursor is over the button

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing!");
            return;
        }

        // Set the default sprite
        spriteRenderer.sprite = normalSprite;
    }

    private void Update()
    {
        if (playerCursor == null)
        {
            Debug.LogError("Player cursor is not assigned!");
            return;
        }

        // Check if the player cursor is overlapping this button
        Vector2 cursorPosition = playerCursor.transform.position;
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null && collider.OverlapPoint(cursorPosition))
        {
            if (!isHovering)
            {
                OnHoverEnter();
            }

            // Check for click input
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                OnClick();
            }
        }
        else
        {
            if (isHovering)
            {
                OnHoverExit();
            }
        }
    }

    private void OnHoverEnter()
    {
        isHovering = true;
        Debug.Log("Hover Start!");
        if (hoverSprite != null)
        {
            spriteRenderer.sprite = hoverSprite;
        }
    }

    private void OnHoverExit()
    {
        isHovering = false;
        Debug.Log("Hover End!");
        if (normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    public void OnClick()
    {
        Debug.Log("Button Clicked!");

        // Change to clicked sprite temporarily
        if (clickedSprite != null)
        {
            spriteRenderer.sprite = clickedSprite;
        }

        // Perform the button action
        PerformAction();

        // Restore hover or normal sprite
        if (isHovering && hoverSprite != null)
        {
            spriteRenderer.sprite = hoverSprite;
        }
        else if (normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    private void PerformAction()
    {
        // Define the button's action here
        Debug.Log($"{gameObject.name}: Action performed!");
    }
}
