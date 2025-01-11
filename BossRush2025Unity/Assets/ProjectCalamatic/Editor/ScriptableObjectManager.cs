using UnityEngine;
using UnityEditor;

public class ScriptableObjectManager
{
    [MenuItem("Assets/Create/My ScriptableObject")]
    public static void CreateAsset()
    {
        var asset = ScriptableObject.CreateInstance<CutsceneDataSO>();
        AssetDatabase.CreateAsset(asset, "Assets/CutsceneDataSO.asset");
        AssetDatabase.SaveAssets();
    }
}
