using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleHandler : MonoBehaviour
{
    public GameObject wheelTab;
    public int totalTurns = 5;
    public float turnTimeLimit = 180f;
    public FortuneWheel fortuneWheel;
    public FolderSO[] folders;
    public RageBar rageBar;

    private GameObject boss;
    private int currentTurn = 0;
    private bool isBossStunned = false;
    private FolderSO chosenFolder;
    private Coroutine turnTimer;

    public GameObject recycleBinPrefab;
    private GameObject recycleBinInstance;

    public TextMeshProUGUI timerText;
    public float timer;

    public PlayerStatSO playerStat;
    public BossStatSo bossStats; // Reference to the BossStats script

    public EndingSO ending;

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
        while (!VDCutsceneManager.isCutscene4Ended)
        {
            yield return null;
        }

        yield return FindBoss();

        Debug.Log("Cutscene 4 finished. Battle starts now!");
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
                yield return null;
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
            turnTimer = StartCoroutine(TurnTimer());

            wheelTab.SetActive(true);
            yield return null;

            yield return fortuneWheel.SpinWheel();

            Debug.Log($"Boss chose folder: {fortuneWheel.selectedFolder.folderName}");
            wheelTab.SetActive(false);

            chosenFolder = fortuneWheel.selectedFolder;
            chosenFolder.currFolderState = FolderStates.inDanger;

            var bossAI = boss.GetComponent<BossAI>();
            if (bossAI != null)
            {
                bossAI.enabled = true;
                Debug.Log("Boss AI started.");
            }
            while (turnTimer != null)
            {
                yield return null;
            }

            Debug.Log($"Turn {currentTurn} ended!");
        }

        Debug.Log("Battle is over!");
        EndBattle();
    }

    private IEnumerator TurnTimer()
    {
        timer = 0f;

        while (timer < turnTimeLimit)
        {
            if (isBossStunned)
            {
                Debug.Log("Boss is stunned! Timer halted.");
                yield return new WaitUntil(() => !isBossStunned);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Turn timer ended! Folder will be deleted.");
        DeleteFolder(chosenFolder);

        turnTimer = null;
    }

    private void DeleteFolder(FolderSO folder)
    {
        if (folder == null) return;

        folder.currFolderState = FolderStates.deleted;
        Debug.Log($"Folder {folder.folderName} has been deleted!");

        if (folder.isGameOverWhenDeleted)
        {
            switch (folder.folderName)
            {
                case "System":
                    ending.currentEnding = Endings.System;
                    break;
                case "Image":
                    ending.currentEnding = Endings.Image;
                    break;
            }
            SceneManager.LoadScene("Ending");
        }
    }

    public void StunBoss()
    {
        if (isBossStunned) return;

        isBossStunned = true;
        Debug.Log("Boss is stunned!");

        var bossAI = boss.GetComponent<BossAI>();
        if (bossAI != null)
        {
            bossAI.enabled = false;
            Debug.Log("Boss AI disabled.");
        }

        rageBar.IncreaseRage();

        // Spawn recycle bin
        recycleBinInstance = Instantiate(recycleBinPrefab, boss.transform.position, Quaternion.identity);
        recycleBinInstance.GetComponent<RecycleBin>().Initialize(this);
    }

    public void RetrieveFolderFromRecycleBin()
    {
        if (!isBossStunned) return;

        Debug.Log($"Player retrieved folder: {chosenFolder.folderName}");
        chosenFolder.currFolderState = FolderStates.retrieved;

        StartCoroutine(BossMovesToRecycleBin());
    }

    private IEnumerator BossMovesToRecycleBin()
    {
        Debug.Log("Boss is moving to the recycle bin...");

        // Simulate boss movement towards the recycle bin
        Vector3 startPosition = boss.transform.position;
        Vector3 endPosition = recycleBinInstance.transform.position;
        float moveSpeed = 3f;
        float journey = 0f;

        while (journey < 1f)
        {
            journey += Time.deltaTime * moveSpeed;
            boss.transform.position = Vector3.Lerp(startPosition, endPosition, journey);
            yield return null;
        }

        Debug.Log("Boss reached the recycle bin.");

        // Destroy the recycle bin
        Destroy(recycleBinInstance);

        // Restore boss's stun bar
        bossStats.bossStunbarHealth = bossStats.bossStunbarHealthMax;
        Debug.Log("Boss stun bar restored to max.");

        // Boss exits stun state
        isBossStunned = false;
        Debug.Log("Boss is no longer stunned. Starting new turn.");

        yield return StartCoroutine(MoveBossToPosition(Vector3.zero));

        StartCoroutine(StartBattle());
    }

    private void DisplayTimer()
    {
        timerText.text = $"{timer:F1}";
    }

    private IEnumerator MoveBossToPosition(Vector3 targetPosition)
    {
        Debug.Log($"Boss is moving to {targetPosition}...");

        Vector3 startPosition = boss.transform.position;
        float moveSpeed = 3f;
        float journey = 0f;

        while (journey < 1f)
        {
            journey += Time.deltaTime * moveSpeed;
            boss.transform.position = Vector3.Lerp(startPosition, targetPosition, journey);
            yield return null;
        }

        Debug.Log($"Boss reached {targetPosition}.");
    }

    private void EndBattle()
    {
        timer = 0f;
        var bossAI = boss.GetComponent<BossAI>();
        if (bossAI != null)
        {
            bossAI.enabled = false;
            Debug.Log("Boss AI disabled.");
        }
        PlayerCursorMovement.Instance.isRequiredCutsceneEnded = false;
        PlayerCursorMovement.Instance.playerHPText.SetActive(false);

        if(FolderManager.numFolderRetreived == 2)
        {
            ending.currentEnding = Endings.DefeatedBossButAtWhatCost;
        }
        else if(FolderManager.numFolderRetreived == 3)
        {
            ending.currentEnding = Endings.DefeatedBoss;
        }
        SceneManager.LoadScene("Ending");
    }

}
