using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform playerTransform;
    public Transform forgeTransform;
    public float heightAbovePlayer = 2.0f;

    void Update()
    {
        // Place the arrow directly above the player
        Vector3 arrowPosition = playerTransform.position + Vector3.up * heightAbovePlayer;
        transform.position = arrowPosition;

        // Calculate the direction from the arrow (not the player) to the forge, at the same height as the arrow
        Vector3 directionToForge = (new Vector3(forgeTransform.position.x, arrowPosition.y, forgeTransform.position.z) - arrowPosition).normalized;

        // Correct the rotation by adding a 90-degree offset around the Y axis
        Quaternion rotationToForge = Quaternion.LookRotation(directionToForge);
        Quaternion correctedRotation = Quaternion.Euler(rotationToForge.eulerAngles.x, rotationToForge.eulerAngles.y - 90, rotationToForge.eulerAngles.z);

        // Apply the corrected rotation
        transform.rotation = correctedRotation;
    }
}
