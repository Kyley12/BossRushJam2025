using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    private bool isEmailTabActive;
    public GameObject emailTab;
    public GameObject startTab;
    private bool isStartTabActive;

    public GameObject myComputerTab;
    private bool isMyComputerActive;

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
        Application.Quit();
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
}
