using UnityEngine;

public class Email : MonoBehaviour
{
    public GameObject recievedEmail1;   // Email 1 object
    public GameObject recievedEmail2;   // Email 2 object
    public GameObject emailNotify;     // Notification mark object on the email
    public bool isEmailViewed = false;

    public static bool emailRecived = false; // Whether a new email has been received
    private int currentlyActivatedEmail = 0;

    private void Update()
    {
        if (emailRecived)
        {
            // Activate the notification mark on the email
            if (emailNotify != null)
            {
                emailNotify.SetActive(true);
            }

            // Trigger the notification pop-up
            NotificationManager.Instance?.ShowNotification("You have a new email!", 2f);

            // Activate the appropriate email object
            if (currentlyActivatedEmail == 0)
            {
                recievedEmail1.SetActive(true);
            }
            else if (currentlyActivatedEmail == 1)
            {
                recievedEmail2.SetActive(true);
            }
            else if (currentlyActivatedEmail > 2)
            {
                return; // No more emails to activate
            }

            // Increment email index and reset the flag
            currentlyActivatedEmail++;
            emailRecived = false;
        }
    }

    public void OnViewedEmail()
    {
        // Deactivate the notification mark if the email is viewed
        if (currentlyActivatedEmail > 0 && emailNotify.activeInHierarchy)
        {
            emailNotify.SetActive(false);

            isEmailViewed = true;
        }
        else
        {
            Debug.Log("It's already viewed email or notification is not active.");
        }
    }
}
