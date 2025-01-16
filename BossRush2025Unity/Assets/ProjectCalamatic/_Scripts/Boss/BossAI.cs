using UnityEngine;

public class BossAI : MonoBehaviour
{
    public TreeNode rootNode;

    private void Update()
    {
        if(rootNode != null)
        {
            rootNode.Execute(this);
        }
    }
}
