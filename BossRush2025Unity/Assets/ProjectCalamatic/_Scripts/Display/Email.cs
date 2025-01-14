using UnityEngine;

public class Email : MonoBehaviour
{
    public GameObject recievedEmail1;
    public GameObject recievedEmail2;
    public GameObject emailNotify;

    public static bool emailRecived;
    public int currentlyActivatedEmail = 0;
    private bool isEmailViewed = false; // New flag to track if the email was viewed

    private void Update()
    {
        if (emailRecived)
        {
            if (currentlyActivatedEmail == 0)
            {
                recievedEmail1.SetActive(true);
                emailNotify.SetActive(true);
            }
            else if (currentlyActivatedEmail == 1)
            {
                recievedEmail2.SetActive(true);
                emailNotify.SetActive(true);
            }
            else if (currentlyActivatedEmail > 2)
            {
                return;
            }
            currentlyActivatedEmail++;
            emailRecived = false;
        }
    }

    public void OnViewedEmail()
    {
        if (currentlyActivatedEmail < 2 && emailNotify.activeInHierarchy)
        {
            emailNotify.SetActive(false);
            isEmailViewed = true; // Mark the email as viewed
        }
        else
        {
            Debug.Log("It's already viewed Email");
        }
    }

    public bool IsEmailViewed()
    {
        return isEmailViewed; // Getter to check if email was viewed
    }
}
