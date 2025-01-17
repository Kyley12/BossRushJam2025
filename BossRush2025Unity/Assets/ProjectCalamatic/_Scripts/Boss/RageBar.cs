using UnityEngine;

public class RageBar : MonoBehaviour
{
    public BossStatSo bossStat;
    public GameObject boss; // Reference to the boss object

    public void IncreaseRage()
    {
        bossStat.bossCurrRage += 20; // Increase rage by a fixed amount
        if (bossStat.bossCurrRage >= bossStat.bossRagebarMax)
        {
            bossStat.bossCurrRage = bossStat.bossRagebarMax;
            TransformBoss(); // Change the boss's appearance
        }

        Debug.Log($"Rage increased! Current rage: {bossStat.bossCurrRage}");
    }

    private void TransformBoss()
    {
        Debug.Log("Boss is enraged! Transforming...");
        // Add logic to change the boss's appearance and behavior here
        boss.GetComponent<SpriteRenderer>().color = Color.red; // Example: Change color to red
    }
}
