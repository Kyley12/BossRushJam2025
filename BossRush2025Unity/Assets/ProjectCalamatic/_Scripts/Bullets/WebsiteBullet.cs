using UnityEngine;
using UnityEngine.SceneManagement;

public class WebsiteBullet : MonoBehaviour
{
    public BulletPattern bulletPattern; // Variables like speed, angle, etc.
    public ScriptableObject websiteBulletDataSO; // To store selected website pattern
    private Vector2 moveDirection;
    public float moveSpeed = 20f;

    public void Initialize(BulletPattern pattern, Vector2 direction, ScriptableObject websiteData)
    {
        bulletPattern = pattern;
        moveDirection = direction;
        websiteBulletDataSO = websiteData;
    }

    private void Update()
    {
        // Move the bullet forward
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCursorMovement.Instance.playerHPText.GetComponent<PlayerStatSO>().DecreaseHealth(bulletPattern.bulletsAmount); //fix this to directly put reference of playerStatSo in this script and every bullet script
            SceneManager.LoadScene("WebsiteScene");
            gameObject.SetActive(false);
        }
    }
}
