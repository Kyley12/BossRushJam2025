using UnityEngine;

[CreateAssetMenu(fileName = "SequenceNode", menuName = "Scriptable Objects/SequenceNode")]
public class SequenceNode : CompositNode
{
    public override bool Execute(BossAI bossAI)
    {
        foreach(TreeNode child in children)
        {
            if(!child.Execute(bossAI))
            {
                return false;
            }
        }
        return true;
    }
}
