using UnityEngine;
using UnityEngine.SceneManagement;

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
        VDCutsceneManager.isCutscene4Ended = false;
        shutdownEnding.SetActive(false);
        PlayerCursorMovement.Instance.isRequiredCutsceneEnded = false;
        PlayerCursorMovement.Instance.playerHPText.SetActive(false);
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

        Invoke("GoBackToStart", 4f);
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

    private void GoBackToStart()
    {
        SceneManager.LoadScene("StartScene");
    }
}
