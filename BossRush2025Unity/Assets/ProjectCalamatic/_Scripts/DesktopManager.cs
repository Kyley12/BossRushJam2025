using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    public GameObject OpenedApp;
    private bool isAppOpened = false;

    public void OnRecycleBin()
    {
        if(isAppOpened)
        {
            OpenedApp.SetActive(false);
            isAppOpened = false;
        }
        else
        {
            OpenedApp.SetActive(true);
            isAppOpened = true;
        }
    }
}
