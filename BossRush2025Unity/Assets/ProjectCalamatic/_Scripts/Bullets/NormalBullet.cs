using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    public PlayerStatSO playerStat;
    public BulletPattern bulletPattern; // Variables like speed, angle, etc.
    private Vector2 moveDirection;
    public float moveSpeed = 20f;

    private Camera mainCamera;

    public void Initialize(BulletPattern pattern, Vector2 direction)
    {
        bulletPattern = pattern;
        moveDirection = direction;
    }

    private void Start()
    {
        mainCamera = Camera.main;
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
            gameObject.SetActive(false);
        }
    }
}
