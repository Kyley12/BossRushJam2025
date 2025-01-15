using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public enum FolderStates
{
    retrieved,
    deleted,
    inDanger
}

public enum ItemType
{
    consumable,
    antivirus,
    key
}

[CreateAssetMenu(fileName = "FolderSO", menuName = "Scriptable Objects/FolderSO")]
public class FolderSO : ScriptableObject
{
    public string folderName;
    public bool isGameOverWhenDeleted;
    public FolderStates currFolderState;
    public List<FolderItems> folderItems;
}

[System.Serializable]
public class FolderItems
{
    public string itemName;
    public ItemType itemType;

    public FolderItems(string itemName, ItemType newItemType)
    {
        this.itemName = itemName;
        this.itemType = newItemType;
    }
}
