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
        if(endingSO.currentEnding == Endings.Shutdowned)
        {
            shutdownEnding.SetActive(true);
            systemEnding.SetActive(false);
            imageEnding.SetActive(false);
            defeatBossButAtWhatCostEnding.SetActive(false);
            defeatBossEnding.SetActive(false);
            cursorBreakEnding.SetActive(false);
        }
        else if(endingSO.currentEnding == Endings.System)
        {
            systemEnding.SetActive(true);
            imageEnding.SetActive(false);
            shutdownEnding.SetActive(false);
            defeatBossButAtWhatCostEnding.SetActive(false);
            defeatBossEnding.SetActive(false);
            cursorBreakEnding.SetActive(false);

        }
        else if(endingSO.currentEnding == Endings.Image)
        {
            imageEnding.SetActive(true);
            shutdownEnding.SetActive(false);
            systemEnding.SetActive(false);
            defeatBossButAtWhatCostEnding.SetActive(false);
            defeatBossEnding.SetActive(false);
            cursorBreakEnding.SetActive(false);
        }
        else if(endingSO.currentEnding == Endings.CursorBreak)
        {
            imageEnding.SetActive(false);
            shutdownEnding.SetActive(false);
            systemEnding.SetActive(false);
            defeatBossButAtWhatCostEnding.SetActive(false);
            defeatBossEnding.SetActive(false);
            cursorBreakEnding.SetActive(true);
        }
        else if(endingSO.currentEnding == Endings.DefeatedBossButAtWhatCost)
        {
            imageEnding.SetActive(false);
            shutdownEnding.SetActive(false);
            systemEnding.SetActive(false);
            defeatBossButAtWhatCostEnding.SetActive(true);
            defeatBossEnding.SetActive(false);
            cursorBreakEnding.SetActive(false);
        }
        else if(endingSO.currentEnding == Endings.DefeatedBoss)
        {
            imageEnding.SetActive(false);
            shutdownEnding.SetActive(false);
            systemEnding.SetActive(false);
            defeatBossButAtWhatCostEnding.SetActive(false);
            defeatBossEnding.SetActive(true);
            cursorBreakEnding.SetActive(false);
        }

        Invoke("GoBackToStart", 4f);
    }

    private void GoBackToStart()
    {
        SceneManager.LoadScene("StartScene");
    }
}
