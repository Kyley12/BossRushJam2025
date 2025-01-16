using UnityEngine;
using System.Collections;

public class FortuneWheel : MonoBehaviour
{
    public Transform wheelBase; // The rotating base of the wheel
    public Transform pointer;  // The pointer that determines the selected segment
    public float spinDuration = 3f; // Duration of the spin
    public float maxSpinSpeed = 500f; // Maximum speed of the spin
    public FolderSO selectedFolder { get; private set; } // The selected folder after spinning
    public LayerMask segmentLayerMask; // LayerMask for valid segment detection

    private bool isSpinning = false; // Track if the wheel is spinning
    private Collider2D[] segmentColliders; // Store segment colliders for quick access

    private void Start()
    {
        // Initialize the active segments
        RefreshActiveSegments();
    }

    public IEnumerator SpinWheel()
    {
        if (isSpinning)
        {
            Debug.LogWarning("The wheel is already spinning!");
            yield break;
        }

        if (segmentColliders.Length == 0)
        {
            Debug.LogError("No active segments to choose from!");
            yield break;
        }

        isSpinning = true;

        float elapsedTime = 0f;
        float currentSpeed = maxSpinSpeed;

        // Calculate the random target rotation
        float randomAngle = Random.Range(0f, 360f);
        float totalRotation = 360f * 3 + randomAngle; // Add full rotations for visual effect

        while (elapsedTime < spinDuration)
        {
            float spinStep = currentSpeed * Time.deltaTime;
            wheelBase.Rotate(Vector3.back, spinStep);

            elapsedTime += Time.deltaTime;

            // Gradually decrease the speed
            currentSpeed = Mathf.Lerp(maxSpinSpeed, 0f, elapsedTime / spinDuration);

            yield return null;
        }

        // Snap to the closest valid segment
        AlignWithClosestSegment();

        // Detect the selected segment
        DetectSelectedSegment();

        isSpinning = false;
    }

    private void AlignWithClosestSegment()
    {
        float currentAngle = wheelBase.eulerAngles.z % 360f;

        // Calculate the closest angle for a valid segment
        float segmentAngle = 360f / segmentColliders.Length;
        float closestAngle = Mathf.Round(currentAngle / segmentAngle) * segmentAngle;

        // Align the wheel to the closest valid angle
        wheelBase.eulerAngles = new Vector3(0, 0, closestAngle);
        Debug.Log($"Wheel aligned to {closestAngle} degrees.");
    }

    private void DetectSelectedSegment()
    {
        Vector3 localPointerPosition = wheelBase.InverseTransformPoint(pointer.position);
        float angle = Mathf.Atan2(localPointerPosition.y, localPointerPosition.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        float segmentAngle = 360f / segmentColliders.Length; // Each segment's angle
        int segmentIndex = Mathf.FloorToInt(angle / segmentAngle);

        if (segmentIndex >= 0 && segmentIndex < segmentColliders.Length)
        {
            Collider2D selectedCollider = segmentColliders[segmentIndex];
            FolderSegment folderSegment = selectedCollider.GetComponent<FolderSegment>();

            if (folderSegment != null)
            {
                selectedFolder = folderSegment.associatedFolder;
                Debug.Log($"Selected folder: {selectedFolder.folderName}");

                // Deactivate the selected segment
                selectedCollider.gameObject.SetActive(false);

                // Refresh active segments
                RefreshActiveSegments();
            }
            else
            {
                Debug.LogError("Selected segment does not have a FolderSegment component.");
            }
        }
        else
        {
            Debug.LogError("No valid segment detected.");
        }
    }

    private void RefreshActiveSegments()
    {
        // Update the list of active segment colliders
        segmentColliders = wheelBase.GetComponentsInChildren<Collider2D>(true);

        if (segmentColliders.Length == 0)
        {
            Debug.LogWarning("No active segments left! All folders have been chosen.");
        }
    }
}