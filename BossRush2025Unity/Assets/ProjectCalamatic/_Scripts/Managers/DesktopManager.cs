using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DesktopManager : MonoBehaviour
{
    public GameObject background;
    public GameObject website;
    public EndingSO endingSO;

    private bool isEmailTabActive;
    public GameObject emailTab;
    public GameObject startTab;
    private bool isStartTabActive;

    public GameObject myComputerTab;
    private bool isMyComputerActive;

    public GameObject finderTab;
    private bool isFinderActive;

    private void Awake()
    {
        SetAllTabToDefault();
        background.SetActive(true);
        website.SetActive(false);
    }

    public void OnEmailPressed()
    {
        if(isEmailTabActive)
        {
            emailTab.SetActive(false);
            isEmailTabActive = false;
        }
        else
        {
            emailTab.SetActive(true);
            isEmailTabActive = true;
            NotificationManager.Instance.HideNotification();
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
        endingSO.currentEnding = Endings.Shutdowned;
        SceneManager.LoadScene("Ending");
    }

    private void SetAllTabToDefault()
    {
        emailTab.SetActive(false);
        startTab.SetActive(false);
    }

    public void OnMyComputerPressed()
    {
        if(isMyComputerActive)
        {
            myComputerTab.SetActive(false);
            isMyComputerActive = false;
        }
        else
        {
            myComputerTab.SetActive(true);
            isMyComputerActive = true;
        }
    }

    public void OnFinderPressed()
    {
        if(isFinderActive)
        {
            finderTab.SetActive(false);
            isFinderActive = false;
        }
        else
        {
            finderTab.SetActive(true);
            isFinderActive = true;
        }
    }
}
