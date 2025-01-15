using UnityEngine;

[CreateAssetMenu(fileName = "TreeNode", menuName = "Scriptable Objects/TreeNode")]
public abstract class TreeNode : ScriptableObject
{
    public abstract bool Execute(BossAI bossAI);
}
