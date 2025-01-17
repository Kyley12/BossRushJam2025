using UnityEngine;
using System.Collections.Generic;

public class NormalBullet : MonoBehaviour
{
     public BulletPattern bulletPattern; // Variables like speed, angle, etc.
    private Vector2 moveDirection;
    public float moveSpeed = 20f;

    public void Initialize(BulletPattern pattern, Vector2 direction)
    {
        bulletPattern = pattern;
        moveDirection = direction;
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
            PlayerCursorMovement.Instance.playerHPText.GetComponent<PlayerStatSO>().DecreaseHealth(bulletPattern.bulletsAmount);
            gameObject.SetActive(false);
        }
    }
}
