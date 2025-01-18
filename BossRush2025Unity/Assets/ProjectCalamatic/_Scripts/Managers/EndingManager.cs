using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public GameObject shutdownEnding;
    public GameObject systemEnding;
    public GameObject imageEnding;

    public EndingSO endingSO;

    private void Start()
    {
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
        }
        else if(endingSO.currentEnding == Endings.System)
        {
            systemEnding.SetActive(true);
            imageEnding.SetActive(false);
            shutdownEnding.SetActive(false);
        }
        else if(endingSO.currentEnding == Endings.Image)
        {
            imageEnding.SetActive(true);
            shutdownEnding.SetActive(false);
            systemEnding.SetActive(false);
        }

        Invoke("GoBackToStart", 4f);
    }

    private void GoBackToStart()
    {
        SceneManager.LoadScene("StartScene");
    }
}
