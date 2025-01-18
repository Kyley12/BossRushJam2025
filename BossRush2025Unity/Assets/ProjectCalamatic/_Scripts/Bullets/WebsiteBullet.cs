using UnityEngine;
using UnityEngine.SceneManagement;

public class WebsiteBullet : MonoBehaviour
{
    public PlayerStatSO playerStat;
    public BulletPattern bulletPattern; // Variables like speed, angle, etc.
    public ScriptableObject websiteBulletDataSO; // To store selected website pattern
    private Vector2 moveDirection;
    public float moveSpeed = 20f;

    private Camera mainCamera;

    public void Initialize(BulletPattern pattern, ScriptableObject websiteData)
    {
        bulletPattern = pattern;
        websiteBulletDataSO = websiteData;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        SetInitialDirection();
    }

    private void Update()
    {
        // Move the bullet forward
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        if(!IsWithinCameraBounds())
        {
            gameObject.SetActive(false);
        }
    }
    private void SetInitialDirection()
    {
        if (PlayerCursorMovement.Instance != null)
        {
            Vector2 playerPosition = PlayerCursorMovement.Instance.GetCursorPosition();
            moveDirection = (playerPosition - (Vector2)transform.position).normalized;
        }
    }

    private bool IsWithinCameraBounds()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerStat.DecreaseHealth(2f);
            SceneManager.LoadScene("WebsiteScene");
            gameObject.SetActive(false);
        }
    }
}
