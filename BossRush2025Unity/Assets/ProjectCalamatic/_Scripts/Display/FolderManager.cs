using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FolderManager : MonoBehaviour
{
    public List<FolderSO> folders;
    public int numFolderRetreived = 0;

    public TextMeshProUGUI systemStatus;
    public TextMeshProUGUI imageStatus;
    public TextMeshProUGUI soundStatus;

    public GameObject inventorySystemFolder;
    public GameObject inventoryImageFolder;
    public GameObject inventorySoundFolder;
    public TextMeshProUGUI numFolderRetreivedText;

    private void Update()
    {
        for (int index = 0; index < folders.Count; index++)
        {
            DisplayMyComputerStatus(index);
            CheckCurrState(index);
        }

        DisplayNumOfFolderRetreived();
    }

    private void DisplayMyComputerStatus(int i)
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

    private void CheckCurrState(int i)
    {
        if(folders[i].currFolderState == FolderStates.retrieved)
        {
            numFolderRetreived++;
            switch(folders[i].folderName)
            {
                case "System":
                    inventorySystemFolder.SetActive(true);
                    break;
                case "Image":
                    inventoryImageFolder.SetActive(true);
                    break;
                case "Sound":
                    inventorySoundFolder.SetActive(true);
                    break;
            }
        }
    }

    private void DisplayNumOfFolderRetreived()
    {
        numFolderRetreivedText.text = "Retereived Folder: \n" + $"{numFolderRetreived} / {folders.Count}";
    }
}
