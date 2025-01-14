using UnityEngine;
using System.Collections;

public class VDCutsceneManager : MonoBehaviour
{
    public Email emailManager; // Reference to the Email script
    public DesktopManager desktopManager; // Reference to the DesktopManager script
    public PlayerCursorMovement playerCursorMovement; // Reference to the PlayerCursorMovement script
    public GameObject bossPrefab; // Boss object to instantiate during Cutscene 2
    public Transform bossSpawnPoint; // Spawn point for the boss
    public GameObject errorMessagePrefab; // Error message prefab for boss's dialogue

    public CutsceneDataSO cutscene1Data; // Dialogues for Cutscene 1
    public CutsceneDataSO cutscene2Data; // Dialogues for Cutscene 2
    public CutsceneDataSO cutscene3Data; // Dialogues for Cutscene 3

    public float emailDelay = 5f; // Delay for sending the email in seconds

    private bool isCutscene1Triggered = false;
    private bool isCutscene1Ended = false;
    private bool isCutscene2Triggered = false;
    private bool isCutscene2Ended = false;
    private bool isCutscene3Triggered = false;

    private void Start()
    {
        // Start Cutscene 1 after the game starts
        StartCoroutine(StartCutscene1());
    }

    private IEnumerator StartCutscene1()
    {
        yield return new WaitForSeconds(emailDelay);
        Email.emailRecived = true; // Trigger the email notification
        isCutscene1Triggered = true;

        // Display Cutscene 1 dialogues (if any)
        if (cutscene1Data != null)
        {
            yield return StartCoroutine(DisplayDialogues(cutscene1Data));
        }

        Debug.Log("Cutscene 1: Email sent to player computer.");
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
    }

    private bool CheckIfEmailViewedAndClosed()
    {
        // Ensure the email app is closed and the email was viewed
        return desktopManager != null &&
               !desktopManager.emailTab.activeInHierarchy &&
               emailManager != null &&
               emailManager.IsEmailViewed();
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
            Debug.Log("Cutscene 3: Player cursor movement changed.");
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
}
