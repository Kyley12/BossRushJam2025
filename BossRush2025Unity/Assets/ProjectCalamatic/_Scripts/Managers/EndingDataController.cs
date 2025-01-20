using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class EndingDataController : MonoBehaviour
{
    [System.Serializable]
    public class EndingData
    {
        public bool shutdownEnding;
        public bool systemEnding;
        public bool imageEnding;
        public bool cursorBreakEnding;
        public bool defeatBossEnding;
        public bool defeatBossButAtWhatCostEnding;
    }

    static EndingDataController _instance;
    public static EndingDataController Instance => _instance;

    public string endingDataFileName = "EDSav.json";

    private EndingData _endingDataFile;
    public EndingData EndingDataFile => _endingDataFile;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            LoadEndingData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        SaveEndingData();
    }

    public void LoadEndingData()
    {
        string filePath = Application.dataPath + "/" + endingDataFileName;

        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);
            _endingDataFile = JsonUtility.FromJson<EndingData>(fromJsonData);
            Debug.Log("Ending data loaded successfully.");
        }
        else
        {
            _endingDataFile = new EndingData();
            SaveEndingData();
            Debug.Log("New ending save file created.");
        }
    }

    public void SaveEndingData()
    {
        string toJsonData = JsonUtility.ToJson(_endingDataFile);
        string filePath = Application.dataPath + "/" + endingDataFileName;

        File.WriteAllText(filePath, toJsonData);
        Debug.Log("Ending data saved successfully.");
    }

    public void UnlockEnding(Endings ending)
    {
        if (_endingDataFile == null)
        {
            Debug.LogWarning("Ending data file is null!");
            return;
        }

        switch (ending)
        {
            case Endings.Shutdowned:
                _endingDataFile.shutdownEnding = true;
                break;
            case Endings.System:
                _endingDataFile.systemEnding = true;
                break;
            case Endings.Image:
                _endingDataFile.imageEnding = true;
                break;
            case Endings.CursorBreak:
                _endingDataFile.cursorBreakEnding = true;
                break;
            case Endings.DefeatedBoss:
                _endingDataFile.defeatBossEnding = true;
                break;
            case Endings.DefeatedBossButAtWhatCost:
                _endingDataFile.defeatBossButAtWhatCostEnding = true;
                break;
        }

        Debug.Log($"Ending unlocked: {ending}");
        SaveEndingData();
    }

    public List<string> GetUnlockedEndings()
    {
        List<string> unlockedEndings = new List<string>();

        if (_endingDataFile.shutdownEnding) unlockedEndings.Add("Shutdowned");
        if (_endingDataFile.systemEnding) unlockedEndings.Add("System");
        if (_endingDataFile.imageEnding) unlockedEndings.Add("Image");
        if (_endingDataFile.cursorBreakEnding) unlockedEndings.Add("CursorBreak");
        if (_endingDataFile.defeatBossEnding) unlockedEndings.Add("DefeatedBoss");
        if (_endingDataFile.defeatBossButAtWhatCostEnding) unlockedEndings.Add("DefeatedBossButAtWhatCost");

        return unlockedEndings;
    }
}
