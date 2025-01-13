using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    private bool isEmailTabActive;
    public static bool isViewedEmail;
    public GameObject emailTab;
    public GameObject startTab;
    private bool isStartTabActive;

    private void Awake()
    {
        SetAllTabToDefault();
    }

    public void OnEmailPressed()
    {
        if(isEmailTabActive)
        {
            emailTab.SetActive(false);
            isEmailTabActive = false;
            if(!VDCutsceneManager.cutscene1Finished && isViewedEmail)
            {
                VDCutsceneManager.emailViewedAndClosed = true;
                isViewedEmail = false;
            }
        }
        else
        {
            emailTab.SetActive(true);
            isEmailTabActive = true;
        }
    }

    public void OnStartButtonPressed()
    {
        if(isStartTabActive)
        {
            startTab.SetActive(false);
            isStartTabActive = false;
        }
        else
        {
            startTab.SetActive(true);
            isStartTabActive = true;
        }
    }

    public void OnShutDownButtonPressed()
    {
        Application.Quit();
    }

    private void SetAllTabToDefault()
    {
        emailTab.SetActive(false);
        startTab.SetActive(false);
    }
}
