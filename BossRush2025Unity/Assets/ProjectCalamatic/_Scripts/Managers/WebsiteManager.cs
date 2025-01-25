using UnityEngine;

public class WebsiteManager : MonoBehaviour
{
    public WebsiteDataSO websiteData;
    public GameObject escapeWay;
    public GameObject website;
    public GameObject background;
    public static GameObject boss;

    public static bool isWebsiteActive;

    private float timer;
    public float maxTime = 5f;

    private void Start()
    {
        background.SetActive(true);
        website.SetActive(false);
    }

    private void Update()
    {
        if (website.activeInHierarchy)
        {
            if (timer > maxTime)
            {
                escapeWay.SetActive(true);
            }

            timer += Time.deltaTime;

            if(!isWebsiteActive)
            {
                website.SetActive(false);
                background.SetActive(true);
                boss.SetActive(true);
            }
        }
        else
        {
            timer = 0f;
            if (isWebsiteActive)
            {
                background.SetActive(false);
                website.SetActive(true);
                boss.SetActive(false);
            }
        }
    }
}
