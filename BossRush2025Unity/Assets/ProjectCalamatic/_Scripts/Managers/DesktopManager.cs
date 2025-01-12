using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    private bool isCurrentTabActive;
    public GameObject emailTab;

    public void OnEmailPressed()
    {
        if(isCurrentTabActive)
        {
            emailTab.SetActive(false);
            isCurrentTabActive = false;
        }
        else
        {
            emailTab.SetActive(true);
            isCurrentTabActive = true;
        }
    }
}
