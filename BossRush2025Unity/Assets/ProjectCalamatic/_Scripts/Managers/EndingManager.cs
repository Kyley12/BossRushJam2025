using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingManager : MonoBehaviour
{
    public GameObject shutdownEnding;
    public GameObject systemEnding;
    public GameObject imageEnding;
    public GameObject cursorBreakEnding;
    public GameObject defeatBossEnding;
    public GameObject defeatBossButAtWhatCostEnding;

    public EndingSO endingSO;

    private void Start()
    {
        WebsiteManager.isWebsiteActive = false;
        VDCutsceneManager.isCutscene4Ended = false;
        BattleHandler.isCriticalFolderDeletd = false;
        shutdownEnding.SetActive(false);
        PlayerCursorMovement.Instance.isRequiredCutsceneEnded = false;
        PlayerCursorMovement.Instance.playerHPText.SetActive(false);
        StartCoroutine(HandleEndingAndGoBack());
    }

    private void Update()
    {
        // Display the active ending and unlock it
        switch (endingSO.currentEnding)
        {
            case Endings.Shutdowned:
                ActivateEnding(shutdownEnding, Endings.Shutdowned);
                break;
            case Endings.System:
                ActivateEnding(systemEnding, Endings.System);
                break;
            case Endings.Image:
                ActivateEnding(imageEnding, Endings.Image);
                break;
            case Endings.CursorBreak:
                ActivateEnding(cursorBreakEnding, Endings.CursorBreak);
                break;
            case Endings.DefeatedBoss:
                ActivateEnding(defeatBossEnding, Endings.DefeatedBoss);
                break;
            case Endings.DefeatedBossButAtWhatCost:
                ActivateEnding(defeatBossButAtWhatCostEnding, Endings.DefeatedBossButAtWhatCost);
                break;
        }
    }

    private void ActivateEnding(GameObject endingObject, Endings ending)
    {
        shutdownEnding.SetActive(false);
        systemEnding.SetActive(false);
        imageEnding.SetActive(false);
        cursorBreakEnding.SetActive(false);
        defeatBossEnding.SetActive(false);
        defeatBossButAtWhatCostEnding.SetActive(false);

        if (endingObject != null)
        {
            endingObject.SetActive(true);
            EndingDataController.Instance.UnlockEnding(ending);
        }
    }

    private IEnumerator HandleEndingAndGoBack()
    {
        // Wait for the ending to finish any animations or transitions
        yield return new WaitForSeconds(4f); // Or adjust this depending on your actual animation time

        Debug.Log("End of ending sequence reached. Transitioning back to start scene.");
        SceneManager.LoadScene("StartScene");
    }
}
