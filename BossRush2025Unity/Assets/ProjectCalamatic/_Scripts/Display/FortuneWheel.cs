using UnityEngine;
using System.Collections;

public class FortuneWheel : MonoBehaviour
{
    public Transform wheelBase; // The rotating base of the wheel
    public Transform pointer;  // The pointer that determines the selected segment
    public float spinDuration = 3f; // Duration of the spin
    public float maxSpinSpeed = 500f; // Maximum speed of the spin
    public FolderSO selectedFolder { get; private set; } // The selected folder after spinning
    public BattleHandler battleHandler; // Reference to the battle handler

    private bool isSpinning = false; // Track if the wheel is spinning
    private Collider2D[] segmentColliders; // Store all segment colliders
    private float segmentAngle; // Angle covered by each segment

    private void Start()
    {
        RefreshActiveSegments();
        segmentAngle = 360f / segmentColliders.Length;
        AlignToFirstSegment();
    }

    public IEnumerator SpinWheel()
    {
        if (isSpinning)
        {
            Debug.LogWarning("The wheel is already spinning!");
            yield break;
        }

        isSpinning = true;

        // Add random starting rotation in multiples of segment angle
        AlignToFirstSegment();
        float randomOffset = Random.Range(0, segmentColliders.Length) * segmentAngle;
        wheelBase.Rotate(Vector3.back, randomOffset);
        Debug.Log($"Random starting rotation: {randomOffset} degrees.");

        float elapsedTime = 0f;
        float currentSpeed = maxSpinSpeed;

        // Perform the spin
        while (elapsedTime < spinDuration)
        {
            float spinStep = currentSpeed * Time.deltaTime;
            wheelBase.Rotate(Vector3.back, spinStep);

            elapsedTime += Time.deltaTime;

            // Gradually decrease the speed
            currentSpeed = Mathf.Lerp(maxSpinSpeed, 0f, elapsedTime / spinDuration);

            yield return null;
        }

        // Snap to the closest valid segment after spinning
        AlignWithClosestSegment();

        // Detect the selected segment
        while (!DetectSelectedSegment())
        {
            // Spin slightly if no valid segment is found
            yield return new WaitForSeconds(0.1f); // Small delay for readability
            wheelBase.Rotate(Vector3.back, 10f); // Rotate slightly to find a valid segment
        }

        isSpinning = false;
    }

    private void AlignToFirstSegment()
    {
        wheelBase.eulerAngles = Vector3.zero; // Align to the first segment
        Debug.Log($"Wheel aligned to the first segment.");
    }

    private void AlignWithClosestSegment()
    {
        float currentAngle = wheelBase.eulerAngles.z % 360f;

        // Calculate the closest angle that aligns with a segment
        float closestAngle = Mathf.Round(currentAngle / segmentAngle) * segmentAngle;

        // Snap the wheel to the closest valid angle
        wheelBase.eulerAngles = new Vector3(0, 0, closestAngle);
        Debug.Log($"Wheel aligned to {closestAngle} degrees.");
    }

    private bool DetectSelectedSegment()
    {
        Vector3 localPointerPosition = wheelBase.InverseTransformPoint(pointer.position);
        float angle = Mathf.Atan2(localPointerPosition.y, localPointerPosition.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        int segmentIndex = Mathf.FloorToInt((angle + (segmentAngle / 2)) % 360f / segmentAngle);

        Debug.Log($"Pointer Angle: {angle}, Calculated Segment Index: {segmentIndex}");

        if (segmentIndex >= 0 && segmentIndex < segmentColliders.Length)
        {
            Collider2D collider = segmentColliders[segmentIndex];

            if (collider != null)
            {
                Debug.Log($"Pointer detected: Collider Name: {collider.name}, Active: {collider.gameObject.activeSelf}");

                if (collider.gameObject.activeSelf)
                {
                    FolderSegment folderSegment = collider.GetComponent<FolderSegment>();

                    if (folderSegment != null)
                    {
                        selectedFolder = folderSegment.associatedFolder;
                        selectedFolder.currFolderState = FolderStates.inDanger;
                        collider.gameObject.SetActive(false); // Deactivate the segment
                        Debug.Log($"Selected Folder: {selectedFolder.folderName}");
                        return true; // Valid segment found
                    }
                    else
                    {
                        Debug.LogWarning($"Collider {collider.name} does not have a FolderSegment script. Continuing spin.");
                        return false; // Invalid segment, continue spinning
                    }
                }
            }
        }

        Debug.LogWarning("No valid segment detected. Pointer landed on an inactive or invalid segment.");
        return false; // Invalid segment, continue spinning
    }

    private void RefreshActiveSegments()
    {
        segmentColliders = wheelBase.GetComponentsInChildren<Collider2D>(true);

        if (segmentColliders.Length == 0)
        {
            Debug.LogWarning("No active segments left! All folders have been chosen.");
        }
    }

    private bool CheckAllFoldersDeactivated()
    {
        foreach (var segmentCollider in segmentColliders)
        {
            if (segmentCollider.gameObject.activeSelf) return false;
        }
        return true; // All segments are deactivated
    }
}