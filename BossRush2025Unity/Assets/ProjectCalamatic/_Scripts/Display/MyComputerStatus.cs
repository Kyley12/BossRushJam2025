using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyComputerStatus : MonoBehaviour
{
    public List<FolderSO> folders;

    public TextMeshProUGUI systemStatus;
    public TextMeshProUGUI imageStatus;
    public TextMeshProUGUI soundStatus;

    private void Update()
    {
        for(int i = 0; i < folders.Count; i++)
        {
            switch(folders[i].folderName)
            {
                case "System":
                    systemStatus.text = "Status: " + folders[i].currFolderState.ToString();
                    break;
                case "Image":
                    imageStatus.text = "Status: " + folders[i].currFolderState.ToString();
                    break;
                case "Sound":
                    soundStatus.text = "Status: " + folders[i].currFolderState.ToString();
                    break;
            }
        }
    }
}
