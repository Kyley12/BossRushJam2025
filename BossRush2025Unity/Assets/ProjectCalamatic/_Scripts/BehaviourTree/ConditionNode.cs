using UnityEngine;

[CreateAssetMenu(fileName = "ConditionNode", menuName = "Scriptable Objects/ConditionNode")]
public class ConditionNode : TreeNode
{
     public string conditionName;

    public override bool Execute(BossAI bossAI)
    {
        switch (conditionName)
        {

            default:
                Debug.LogWarning($"Unknown condition: {conditionName}");
                return false;
        }
    }
}
