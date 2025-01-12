using UnityEngine;
using UnityEditor;

public class ScriptableObjectManager
{
    [MenuItem("Assets/Create/ScriptableObject")]
    public static void CreateAsset()
    {
        var asset = ScriptableObject.CreateInstance<CutsceneDataSO>();
        AssetDatabase.CreateAsset(asset, "Assets/ProjectCalamatic/Resources/CutsceneDataSO.asset");
        AssetDatabase.SaveAssets();
    }
}
