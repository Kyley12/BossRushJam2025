using UnityEngine;
using UnityEngine.Events;

public class SpriteButton : MonoBehaviour
{
    public Sprite normalSprite;  // Default button sprite
    public Sprite hoverSprite;   // Sprite when hovering
    public Sprite clickedSprite; // Sprite when clicked

    public UnityEvent onClick; // Event triggered when the button is clicked

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
        // Detect the player cursor's position
        Vector2 playerCursorPosition = PlayerCursorMovement.Instance.GetCursorPosition();

        // Check if the player cursor overlaps the button's collider
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && collider.OverlapPoint(playerCursorPosition))
        {
            if (!isHovering)
            {
                OnHoverEnter();
            }

            // Detect click input
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
        if (hoverSprite != null)
        {
            spriteRenderer.sprite = hoverSprite;
        }
    }

    private void OnHoverExit()
    {
        isHovering = false;
        if (normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    private void OnClick()
    {
        Debug.Log($"Button Clicked: {gameObject.name}");

        // Change to clicked sprite temporarily
        if (clickedSprite != null)
        {
            spriteRenderer.sprite = clickedSprite;
        }

        // Trigger the assigned action
        onClick?.Invoke();

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
}
