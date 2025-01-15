using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CompositNode", menuName = "Scriptable Objects/CompositNode")]
public abstract class CompositNode : TreeNode
{ 
    public List<TreeNode> children = new List<TreeNode>();
}
