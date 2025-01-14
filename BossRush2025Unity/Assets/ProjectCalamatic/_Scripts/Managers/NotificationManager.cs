using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    public GameObject notificationPrefab; // Reference to the notification prefab (sprite-based)
    public Transform notificationSpawnPoint; // World-space position to spawn notifications
    public float notificationDuration = 2f; // Duration the notification is displayed

    private GameObject currentNotification; // Reference to the current active notification
    private Coroutine notificationCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Show a world-space notification.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="duration">Optional duration for the notification.</param>
    public void ShowNotification(string message, float duration = -1f)
    {
        if (duration <= 0f)
        {
            duration = notificationDuration;
        }

        // Destroy any existing notification
        if (currentNotification != null)
        {
            Destroy(currentNotification);
        }

        // Stop any existing coroutine
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }

        // Instantiate the notification prefab
        currentNotification = Instantiate(notificationPrefab, notificationSpawnPoint.position, Quaternion.identity);

        // Set the notification message
        var textComponent = currentNotification.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = message;
        }

        // Start the coroutine to hide the notification
        notificationCoroutine = StartCoroutine(AutoHideNotification(duration));
    }

    private IEnumerator AutoHideNotification(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Destroy the notification after the duration
        if (currentNotification != null)
        {
            Destroy(currentNotification);
            currentNotification = null;
        }
    }

    /// <summary>
    /// Hide the current notification manually.
    /// </summary>
    public void HideNotification()
    {
        if (currentNotification != null)
        {
            Destroy(currentNotification);
            currentNotification = null;
        }

        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
    }
}
