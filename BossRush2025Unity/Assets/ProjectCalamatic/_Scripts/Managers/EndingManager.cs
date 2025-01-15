using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public GameObject shutdownEnding;
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
            Invoke("GoBackToStart", 4f);
        }
    }

    private void GoBackToStart()
    {
        SceneManager.LoadScene("StartScene");
    }
}
