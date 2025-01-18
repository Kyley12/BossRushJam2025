using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleHandler : MonoBehaviour
{
    public GameObject wheelTab; // Reference to the fortune wheel UI
    public int totalTurns = 5; // Total number of turns in the game
    public float turnTimeLimit = 180f; // Time limit for each turn
    public FortuneWheel fortuneWheel; // Reference to the FortuneWheel script
    public FolderSO[] folders; // List of folder ScriptableObjects
    public RageBar rageBar; // Reference to the rage bar system
    public VDCutsceneManager cutsceneManager; // Reference to the VDCutsceneManager

    private GameObject boss; // Reference to the boss object
    private int currentTurn = 0; // Track the current turn
    private bool isBossStunned = false; // Track if the boss is stunned
    private FolderSO chosenFolder; // Folder chosen by the wheel spin
    private Coroutine turnTimer; // Coroutine reference for turn timer

    public TextMeshProUGUI timerText;
    public float timer;

    public PlayerStatSO playerStat;

    public EndingSO endingSo;

    public static bool isTimeHalted = false;

    private void Awake()
    {
        playerStat.cursorHealth = playerStat.cursorMaxHealth;
    }

    private void Start()
    {
        StartCoroutine(WaitForCutsceneAndStartBattle());
    }

    private void Update()
    {
        DisplayTimer();
    }

    private IEnumerator WaitForCutsceneAndStartBattle()
    {
        // Wait for Cutscene 4 to finish
        while (!VDCutsceneManager.isCutscene4Ended)
        {
            yield return null;
        }

        // Dynamically find the boss in the scene
        yield return FindBoss();

        Debug.Log("Cutscene 4 finished. Battle starts now!");

        // Start the battle after Cutscene 4
        StartCoroutine(StartBattle());
    }

    private IEnumerator FindBoss()
    {
        while (boss == null)
        {
            boss = GameObject.FindWithTag("Boss");

            if (boss == null)
            {
                Debug.Log("Waiting for boss to spawn...");
                yield return null; // Wait for the next frame
            }
        }

        Debug.Log("Boss found!");
    }

    private IEnumerator StartBattle()
    {
        while (currentTurn < totalTurns)
        {
            currentTurn++;
            Debug.Log($"Turn {currentTurn} begins!");

            // Show the fortune wheel
            wheelTab.SetActive(true);

            // Wait for one frame to ensure wheelTab components are initialized
            yield return null;

            // Spin the wheel to choose a folder
            yield return fortuneWheel.SpinWheel();

            Debug.Log($"Boss chose folder: {fortuneWheel.selectedFolder.folderName}");
        
            // Hide the wheel tab after spinning
            wheelTab.SetActive(false);

            // Start the turn logic (folder marked as in danger)
            chosenFolder = fortuneWheel.selectedFolder;
            chosenFolder.currFolderState = FolderStates.inDanger;

            // Start the boss AI behavior
            var bossAI = boss.GetComponent<BossAI>();
            if (bossAI != null)
            {
                bossAI.enabled = true; // Enable the Boss AI
                Debug.Log("Boss AI started.");
            }

            // Start the turn timer
            turnTimer = StartCoroutine(TurnTimer());

            // Wait for the turn to end
            while (turnTimer != null)
            {
                yield return null;
            }

            Debug.Log($"Turn {currentTurn} ended!");
        }

        Debug.Log("Battle is over!");
    }

    private IEnumerator TurnTimer()
    {
        timer = 0f;

        while (timer < turnTimeLimit)
        {
            if (isBossStunned)
            {
                Debug.Log("Boss is stunned! Timer paused.");
                yield return new WaitUntil(() => !isBossStunned); // Wait until boss is no longer stunned
            }

            
            timer += Time.deltaTime;
            
            yield return null;
        }

        Debug.Log("Turn timer ended! Folder will be deleted.");
        DeleteFolder(chosenFolder);

        turnTimer = null; // End the turn
    }

    private void DeleteFolder(FolderSO folder)
    {
        if (folder == null) return;

        folder.currFolderState = FolderStates.deleted;
        Debug.Log($"Folder {folder.folderName} has been deleted!");

        // Check if game should end
        if (folder.isGameOverWhenDeleted)
        {
            Debug.LogError("Game Over! Critical folder was deleted.");

            switch (folder.folderName)
            {
                case "System":
                    endingSo.currentEnding = Endings.System;
                    break;
                case "Image":
                    endingSo.currentEnding = Endings.Image;
                    break;
            }

            SceneManager.LoadScene("Ending");
            // Trigger game over logic here
        }
    }

    public void StunBoss()
    {
        if (isBossStunned) return;

        isBossStunned = true;
        Debug.Log("Boss is stunned!");

        // Rage bar increases when boss loses
        rageBar.IncreaseRage();

        // Player can interact with the recycle bin to recover the folder
    }

    public void RetrieveFolderFromRecycleBin()
    {
        if (!isBossStunned) return;

        Debug.Log($"Player retrieved folder: {chosenFolder.folderName}");
        chosenFolder.currFolderState = FolderStates.retrieved;

        // End the boss's stun
        isBossStunned = false;
        Debug.Log("Boss is no longer stunned!");
    }

    private void DisplayTimer()
    {
        timerText.text = $"{timer}";
    }

    public void EndBattle()
    {

    }
    
}
