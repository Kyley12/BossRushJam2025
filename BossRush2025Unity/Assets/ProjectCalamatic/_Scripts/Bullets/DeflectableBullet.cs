using Codice.Client.BaseCommands.Merge.Restorer.Finder;
using UnityEngine;

public class DeflectableBullet : MonoBehaviour
{
    public BulletPattern bulletPattern; // Variables like speed, etc.
    public BossStatSo bossStats; // Reference to boss stats
    private Vector2 moveDirection; // Current move direction
    public float moveSpeed = 20f;
    private bool isDeflected = false; // Whether the bullet has been deflected

    private void Start()
    {
        // Ensure only one deflectable bullet exists at a time
        if (FindObjectsOfType<DeflectableBullet>().Length > 1)
        {
            Destroy(gameObject);
        }

        SetInitialDirection();
    }

    private void Update()
    {
        // Dynamically update the direction if not deflected
        if (!isDeflected && PlayerCursorMovement.Instance != null)
        {
            Vector2 playerPosition = PlayerCursorMovement.Instance.GetCursorPosition();
            moveDirection = (playerPosition - (Vector2)transform.position).normalized;
        }

        // Move the bullet
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Initializes the bullet with its pattern and boss stats.
    /// </summary>
    /// <param name="pattern">Bullet pattern data.</param>
    /// <param name="bossStat">Reference to the boss stats.</param>
    public void Initialize(BulletPattern pattern, BossStatSo bossStat)
    {
        bulletPattern = pattern;
        bossStats = bossStat;
    }

    /// <summary>
    /// Sets the initial direction towards the player.
    /// </summary>
    private void SetInitialDirection()
    {
        if (PlayerCursorMovement.Instance != null)
        {
            Vector2 playerPosition = PlayerCursorMovement.Instance.GetCursorPosition();
            moveDirection = (playerPosition - (Vector2)transform.position).normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Bullet is deflected
            isDeflected = true;
            moveDirection = -moveDirection; // Reverse the direction
        }
        else if (collision.gameObject.CompareTag("Boss") && isDeflected)
        {
            // Decrease the boss's stun bar
            bossStats.bossStunbarHealth -= bulletPattern.bulletsAmount;
            if (bossStats.bossStunbarHealth < 0)
                bossStats.bossStunbarHealth = 0;

            gameObject.SetActive(false); // Deactivate bullet after hitting the boss
        }
    }
}
