using UnityEngine;

public class Email : MonoBehaviour
{
    public GameObject recievedEmail1;
    public GameObject recievedEmail2;
    public GameObject emailNotify;

    public static bool emailRecived;
    private int currentlyActivatedEmail = 0;

    private void Update()
    {
        if(emailRecived)
        {
            if(currentlyActivatedEmail == 0)
            {
                recievedEmail1.SetActive(true);
                emailNotify.SetActive(true);
            }
            else if(currentlyActivatedEmail == 1)
            {
                recievedEmail2.SetActive(true);
                emailNotify.SetActive(true);
            }
            else if(currentlyActivatedEmail > 2)
            {
                return;
            }
            currentlyActivatedEmail++;
            emailRecived = false;
        }
    }

    public void OnViewedEmail()
    {
        if(currentlyActivatedEmail > 0 && emailNotify.activeInHierarchy)
        {
            emailNotify.SetActive(false);
            DesktopManager.isViewedEmail = true;
        }
        else
        {
            Debug.Log("It's already viewed Email");
        }
    }
}
