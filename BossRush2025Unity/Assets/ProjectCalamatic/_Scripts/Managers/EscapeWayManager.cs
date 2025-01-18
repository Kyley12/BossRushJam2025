using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using JetBrains.Annotations;

public class EscapeWayManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            WebsiteManager.isWebsiteActive = false;
            gameObject.SetActive(false);
        }
    }
}


