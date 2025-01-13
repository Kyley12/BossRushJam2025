using System.Collections;
using TMPro;
using UnityEngine;

public class VDCutsceneManager : MonoBehaviour
{
    public GameObject testText1;
    public GameObject testText2;
    public GameObject boss;
    public Transform defaultBossPos;
    public static bool emailViewedAndClosed;

    public static bool cutscene1Finished;
    private bool cutscene2Finished;
    private void Start()
    {
        // Start the delayed cutscene logic
        StartCoroutine(StartCutscene1AfterDelay(5f));
    }

    private void Update()
    {
        if(emailViewedAndClosed)
        {
            testText1.SetActive(true);
            Invoke("FinishCutscene1Animation", 2);
            

            emailViewedAndClosed = false;
        }

        if(cutscene1Finished && !cutscene2Finished)
        {
            Invoke("StartCutscene2AfterDelay", 2);
            Invoke("FinishCutscene2Animation", 4);
        }
    }

    private IEnumerator StartCutscene1AfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Notify the Email script
        Email.emailRecived = true;

        Debug.Log("Cutscene started and emailRecieved set to true.");
    }

    private void FinishCutscene1Animation()
    {
        testText1.SetActive(false);
        BossComesIn();
        cutscene1Finished = true;
    }
    
    private void StartCutscene2AfterDelay()
    {
        testText2.SetActive(true);
    }
    private void FinishCutscene2Animation()
    {
        testText2.SetActive(false);
        PlayerCursorMovement.Instance.isRequiredCutsceneEnded = true;
        cutscene2Finished = true;
    }
    private void BossComesIn()
    {
        Instantiate(boss, defaultBossPos);
    }

}
