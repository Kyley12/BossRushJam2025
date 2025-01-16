using UnityEngine;

public class FolderSegment : MonoBehaviour
{
    public FolderSO associatedFolder; // The folder associated with this segment

    private void Start()
    {
        // Ensure the associated folder is assigned
        if (associatedFolder == null)
        {
            Debug.LogError($"No folder assigned to segment {gameObject.name}. Please assign a FolderSO.");
        }
    }

    public FolderSO GetFolder()
    {
        return associatedFolder;
    }
}
