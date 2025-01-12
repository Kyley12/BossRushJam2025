using UnityEngine;
using UnityEditor;
using System.IO;

public class CutsceneEditorManagr
{
    public CutsceneDataSO cutsceneDataSO;

    [MenuItem("Data IO/Generate Txt File")]
    public static void GenerateTxtFile()
    {
        // Ensure directory exists
        string directoryPath = Application.dataPath + "/ProjectCalamatic/Resources/Files/TxtFiles";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using (StreamWriter generatefile = new StreamWriter(directoryPath + "/OpeningCutsceneData.txt"))
        {
            // Example of writing initial content (optional)
            generatefile.WriteLine("Example Cutscene Line 1");
            generatefile.WriteLine("Example Cutscene Line 2");
        }
    }

    [MenuItem("Data IO/Load Data")]
    public static void LoadData()
    {
        // Load the CutsceneDataSO ScriptableObject from Resources
        CutsceneDataSO cutsceneDataSO = Resources.Load<CutsceneDataSO>("OpeningCutsceneDataSO");
        if (cutsceneDataSO == null)
        {
            Debug.LogError("CutsceneDataSO could not be loaded. Ensure it is in the Resources folder.");
            return;
        }

        // Clear existing data to avoid duplication
        cutsceneDataSO.ClearLineData();

        string filePath = Application.dataPath + "/ProjectCalamatic/Resources/Files/TxtFiles/OpeningCutsceneData.txt";
        if (!File.Exists(filePath))
        {
            Debug.LogError("OpeningCutsceneData.txt not found at " + filePath);
            return;
        }

        // Read the file line by line and add to the ScriptableObject
        using (StreamReader reader = new StreamReader(filePath))
        {
            string data;
            while ((data = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(data))
                {
                    cutsceneDataSO.PutStringDataIntoLineData(data);
                }
            }
        }

        Debug.Log("Cutscene data loaded successfully. Total lines: " + cutsceneDataSO.lineData.Count);
    }
}
