using UnityEngine;

public class BossAI : MonoBehaviour
{
    public TreeNode rootNode;

    public static Vector2 bossPos;

    private void Update()
    {
        bossPos = transform.position;
        if(rootNode != null)
        {
            rootNode.Execute(this);
        }
    }
}
