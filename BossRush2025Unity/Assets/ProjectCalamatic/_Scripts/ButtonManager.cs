using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public LayerMask buttonLayer; // Only detect buttons

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Raycast to detect buttons
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, Mathf.Infinity, buttonLayer);
            if (hit.collider != null)
            {
                SpriteButton button = hit.collider.GetComponent<SpriteButton>();
                if (button != null)
                {
                    button.OnClick();
                }
            }
        }
    }
}
