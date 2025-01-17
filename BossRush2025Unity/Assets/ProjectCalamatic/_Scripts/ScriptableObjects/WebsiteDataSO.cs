using UnityEngine;

public enum WebsiteType
{
    Google
}
[CreateAssetMenu(fileName = "WebsiteDataSO", menuName = "Scriptable Objects/WebsiteDataSO")]
public class WebsiteDataSO : ScriptableObject
{
    public WebsiteType currentWebsiteType;
}
