using System.Collections;
using UnityEngine;

public class VDCutsceneManager : MonoBehaviour
{
    public static bool emailViewedAndClosed;
    private void Start()
    {
        // Start the delayed cutscene logic
        StartCoroutine(StartCutscene1AfterDelay(5f));
    }

    private void Update()
    {
        if(emailViewedAndClosed)
        {
            Debug.Log("Play Cutscene 2 Timeline");
        }
    }

    private IEnumerator StartCutscene1AfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Notify the Email script
        Email.emailRecived = true;

        Debug.Log("Cutscene started and emailRecieved set to true.");
    }


}
