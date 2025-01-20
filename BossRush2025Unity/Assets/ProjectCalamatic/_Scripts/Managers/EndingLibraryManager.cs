using System.Collections.Generic;
using UnityEngine;

public class EndingLibraryManager : MonoBehaviour
{
    public GameObject endingShutdowned;
    public GameObject endingSystem;
    public GameObject endingImage;
    public GameObject endingCursorBreak;
    public GameObject endingDefeatedBoss;
    public GameObject endingDefeatedBossButAtWhatCost;

    public List<string> endingDatas = new List<string>();
    private void Awake()
    {
        endingShutdowned.SetActive(false);
        endingSystem.SetActive(false);
        endingImage.SetActive(false);
        endingCursorBreak.SetActive(false);
        endingDefeatedBoss.SetActive(false);
        endingDefeatedBossButAtWhatCost.SetActive(false);
    }
    private void Update()
    {
        if (EndingDataController.Instance.GetUnlockedEndings() != null)
        {
            endingDatas = EndingDataController.Instance.GetUnlockedEndings();

            for (int i = 0; i < endingDatas.Count; i++)
            {
                switch (endingDatas[i])
                {
                    case "Shutdowned":
                        endingShutdowned.SetActive(true);
                        break;
                    case "System":
                        endingSystem.SetActive(true);
                        break;
                    case "Image":
                        endingImage.SetActive(true);
                        break;
                    case "CursorBreak":
                        endingCursorBreak.SetActive(true);
                        break;
                    case "DefeatedBoss":
                        endingDefeatedBoss.SetActive(true);
                        break;
                    case "DefeatedBossButAtWhatCost":
                        endingDefeatedBossButAtWhatCost.SetActive(true);
                        break;

                }
            }
        }
    }
}
