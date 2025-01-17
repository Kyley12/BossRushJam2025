using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatSO", menuName = "Scriptable Objects/PlayerStatSO")]
public class PlayerStatSO : ScriptableObject
{
    [Header("Player Health")]
    public float cursorHealth;
    public int cursorMaxHealth;

    [Header("Player Inventory")]
    public List<string> folders;

    /// <summary>
    /// Decreases the player's cursor health by a specified amount.
    /// </summary>
    /// <param name="amount">Amount of health to decrease.</param>
    public void DecreaseHealth(float amount)
    {
        cursorHealth -= amount;
        if (cursorHealth < 0)
        {
            cursorHealth = 0; // Prevent health from going below zero
        }
    }

    /// <summary>
    /// Restores the player's cursor health by a specified amount.
    /// </summary>
    /// <param name="amount">Amount of health to restore.</param>
    public void RestoreHealth(int amount)
    {
        cursorHealth += amount;
        if (cursorHealth > cursorMaxHealth)
        {
            cursorHealth = cursorMaxHealth; // Cap health at the maximum
        }
    }

    /// <summary>
    /// Checks if the player's cursor health is at zero.
    /// </summary>
    /// <returns>True if health is zero, false otherwise.</returns>
    public bool IsHealthDepleted()
    {
        return cursorHealth <= 0;
    }

    /// <summary>
    /// Adds a new folder to the player's inventory.
    /// </summary>
    /// <param name="folderName">The name of the folder to add.</param>
    public void AddFolder(string folderName)
    {
        if (!folders.Contains(folderName))
        {
            folders.Add(folderName);
        }
    }

    /// <summary>
    /// Removes a folder from the player's inventory.
    /// </summary>
    /// <param name="folderName">The name of the folder to remove.</param>
    public void RemoveFolder(string folderName)
    {
        if (folders.Contains(folderName))
        {
            folders.Remove(folderName);
        }
    }

    /// <summary>
    /// Checks if a folder exists in the player's inventory.
    /// </summary>
    /// <param name="folderName">The name of the folder to check.</param>
    /// <returns>True if the folder exists, false otherwise.</returns>
    public bool HasFolder(string folderName)
    {
        return folders.Contains(folderName);
    }
}
