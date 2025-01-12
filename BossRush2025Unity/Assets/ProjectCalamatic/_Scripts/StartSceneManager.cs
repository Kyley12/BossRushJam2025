using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    public GameObject endingPanel;
    private bool isEndingPanelActive = false;
    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnEndingLibraray()
    {
        if (isEndingPanelActive)
        {
            endingPanel.SetActive(false);
            isEndingPanelActive = false;
        }
        else
        {
            endingPanel.SetActive(true);
            isEndingPanelActive = true;
        }
    }
}
