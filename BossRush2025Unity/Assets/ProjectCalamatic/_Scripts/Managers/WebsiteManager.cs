using UnityEngine;

public class WebsiteManager : MonoBehaviour
{
    public WebsiteDataSO websiteData;
    public GameObject escapeWay;
    public GameObject website;
    public GameObject background;

    public static bool isWebsiteActive;

    private float timer;
    public float maxTime = 5f;

    private void Start()
    {
        background.SetActive(true);
        website.SetActive(false);
        switch (websiteData.currentWebsiteType.ToString())
        {
            case "SearchEngine":
                StartCoroutine("SearchEnginePattern");
                break;
        }
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
            }
        }
        else
        {
            timer = 0f;
            if (isWebsiteActive)
            {
                background.SetActive(false);
                website.SetActive(true);
            }
        }
    }

    private void SearchEnginePattern()
    {

    }
}
