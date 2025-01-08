using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneDataSO", menuName = "Scriptable Objects/CutsceneDataSO")]
public class CutsceneDataSO : ScriptableObject
{
    public List<string> lineData = new List<string>();

    public void PutStringDataIntoLineData(string data)
    {
        if(lineData == null)
        {
            lineData = new List<string>();
        }
        lineData.Add(data);
    }

    public void ClearLineData()
    {
        if (lineData != null)
        {
            lineData.Clear(); // Clear the list if needed
        }
    }
}
