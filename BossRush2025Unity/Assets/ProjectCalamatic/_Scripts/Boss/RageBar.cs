using UnityEngine;

public class RageBar : MonoBehaviour
{
    public int maxRage = 100; // Maximum rage value
    public int currentRage = 0; // Current rage value
    public GameObject boss; // Reference to the boss object

    public void IncreaseRage()
    {
        currentRage += 20; // Increase rage by a fixed amount
        if (currentRage >= maxRage)
        {
            currentRage = maxRage;
            TransformBoss(); // Change the boss's appearance
        }

        Debug.Log($"Rage increased! Current rage: {currentRage}");
    }

    private void TransformBoss()
    {
        Debug.Log("Boss is enraged! Transforming...");
        // Add logic to change the boss's appearance and behavior here
        boss.GetComponent<SpriteRenderer>().color = Color.red; // Example: Change color to red
    }
}
