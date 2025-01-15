using UnityEngine;

[CreateAssetMenu(fileName = "SelectionNode", menuName = "Scriptable Objects/SelectionNode")]
public class SelectionNode : CompositNode
{
    public override bool Execute(BossAI bossAI)
    {
        foreach(TreeNode child in children)
        {
            if(child.Execute(bossAI))
            {
                return true;
            }
        }
        return false;
    }
}
