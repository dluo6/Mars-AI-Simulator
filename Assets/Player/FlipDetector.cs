using UnityEngine;

public class FlipDetector : MonoBehaviour
{
    public float flipThreshold = 80f;
    public float respawnHeight = 3f;

    private void Update()
    {
        // Check if rover is upside down
        if (Vector3.Angle(transform.up, Vector3.up) > flipThreshold)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        // Reset position and rotation
        transform.position += Vector3.up * respawnHeight;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0); // Keep only yaw

        // Reset physics if exists
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}