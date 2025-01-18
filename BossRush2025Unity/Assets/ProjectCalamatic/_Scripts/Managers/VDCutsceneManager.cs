using UnityEngine;
using System.Collections;
using TMPro;

public class VDCutsceneManager : MonoBehaviour
{
    public Email emailManager; // Reference to the Email script
    public DesktopManager desktopManager; // Reference to the DesktopManager script
    public GameObject bossPrefab; // Boss object to instantiate during Cutscene 2
    public Transform bossSpawnPoint; // Spawn point for the boss
    public GameObject errorMessagePrefab; // Error message prefab for boss's dialogue
    public CutsceneDataSO cutscene2Data; // Dialogues for Cutscene 2
    public CutsceneDataSO cutscene3Data; // Dialogues for Cutscene 3
    public CutsceneDataSO cutscene4Data;
    public float emailDelay = 5f; // Delay for sending the email in seconds
    public GameObject recycleBinIcon; // Reference to the Recycle Bin icon

    private bool isCutscene1Triggered = false;
    private bool isCutscene1Ended = false;
    private bool isCutscene2Triggered = false;
    private bool isCutscene2Ended = false;
    private bool isCutscene3Triggered = false;
    private bool isCutscene3Ended = false;
    private bool isCutscene4Triggered = false;
    public static bool isCutscene4Ended = false;

    private PlayerCursorMovement playerCursorMovement; // Reference to the player cursor
    private Vector3 bossInitialPosition; // Boss's initial position

    private void Start()
    {
        // Dynamically find the PlayerCursorMovement instance
        AssignPlayerCursorMovement();

        // Start Cutscene 1 after the game starts
        StartCoroutine(StartCutscene1());
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        // Ensure Cutscene 1 is fully completed
        if (isCutscene1Triggered && !isCutscene1Ended && CheckIfEmailViewedAndClosed())
        {
            isCutscene1Ended = true;
        }

        // Trigger Cutscene 2 only if Cutscene 1 has ended
        if (isCutscene1Ended && !isCutscene2Triggered)
        {
            StartCoroutine(StartCutscene2());
        }

        // Trigger Cutscene 3 only if Cutscene 2 has ended
        if (isCutscene2Ended && !isCutscene3Triggered)
        {
            StartCoroutine(StartCutscene3());
        }

         if (isCutscene3Ended && !isCutscene4Triggered)
        {
            StartCoroutine(StartCutscene4());
        }
    }

    private bool CheckIfEmailViewedAndClosed()
    {
        // Ensure the email app is closed and the email was viewed
        return desktopManager != null &&
               !desktopManager.emailTab.activeInHierarchy &&
               emailManager != null &&
               emailManager.isEmailViewed;
    }

    private IEnumerator StartCutscene1()
    {
        yield return new WaitForSeconds(emailDelay);
        Email.emailRecived = true; // Trigger the email notification
        isCutscene1Triggered = true;

        NotificationManager.Instance.ShowNotification("New notification", 2f);

        Debug.Log("Cutscene 1: Email sent to player computer.");
    }

    private IEnumerator StartCutscene2()
    {
        isCutscene2Triggered = true;

        // Display error messages containing boss's dialogue (one at a time)
        if (cutscene2Data != null)
        {
            yield return StartCoroutine(DisplayDialogues(cutscene2Data));
        }

        // Instantiate the boss in the scene
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            Debug.Log("Cutscene 2: Boss instantiated.");
        }

        // Mark Cutscene 2 as ended
        isCutscene2Ended = true;
    }

    private IEnumerator StartCutscene3()
    {
        isCutscene3Triggered = true;

        // Display the error message indicating cursor movement change (one at a time)
        if (cutscene3Data != null)
        {
            yield return StartCoroutine(DisplayDialogues(cutscene3Data));
        }

        // Change the cursor movement logic
        if (playerCursorMovement != null)
        {
            playerCursorMovement.isRequiredCutsceneEnded = true;
            playerCursorMovement.playerHPText.SetActive(true);
            Debug.Log("Cutscene 3: Player cursor movement changed.");
        }

        isCutscene3Ended = true;
    }

     private IEnumerator StartCutscene4()
    {
        isCutscene4Triggered = true;

        // Ensure the boss and recycle bin icon are set
        if (bossPrefab == null || recycleBinIcon == null)
        {
            Debug.LogError("Boss or Recycle Bin Icon not set!");
            yield break;
        }

        GameObject boss = GameObject.FindWithTag("Boss");
        if (boss == null)
        {
            Debug.LogError("Boss not found in the scene!");
            yield break;
        }

        bossInitialPosition = boss.transform.position;

        // Move the boss to the recycle bin
        yield return MoveToPosition(boss, recycleBinIcon.transform.position);

        // Simulate picking up the recycle bin
        Debug.Log("Boss picked up the Recycle Bin!");
        recycleBinIcon.SetActive(false); // Hide the recycle bin icon

        // Move the boss back to its initial position
        yield return MoveToPosition(boss, bossInitialPosition);

        // Display dialogues for Cutscene 4
        if (cutscene4Data != null)
        {
            yield return StartCoroutine(DisplayDialogues(cutscene4Data));
        }

        Debug.Log("Cutscene 4 completed!");
        isCutscene4Ended = true;
    }

    private IEnumerator MoveToPosition(GameObject obj, Vector3 targetPosition)
    {
        float speed = 2f; // Adjust movement speed
        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.1f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator DisplayDialogues(CutsceneDataSO cutsceneData)
    {
        if (cutsceneData == null || cutsceneData.lineData == null) yield break;

        foreach (var dialogue in cutsceneData.lineData)
        {
            GameObject errorMessage = Instantiate(errorMessagePrefab);
            var textComponent = errorMessage.GetComponentInChildren<TMPro.TextMeshProUGUI>();

            if (textComponent != null)
            {
                textComponent.text = dialogue;
            }

            yield return new WaitForSeconds(2f); // Wait for the dialogue to be displayed
            Destroy(errorMessage); // Destroy the error message after it is displayed
        }
    }

    private void AssignPlayerCursorMovement()
    {
        // Dynamically find the PlayerCursorMovement instance
        playerCursorMovement = PlayerCursorMovement.Instance;

        if (playerCursorMovement == null)
        {
            Debug.LogError("PlayerCursorMovement instance not found! Ensure the PlayerCursor is properly set in the scene.");
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        AssignPlayerCursorMovement(); // Reassign PlayerCursorMovement on scene load
    }
}
