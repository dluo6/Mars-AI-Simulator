using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform rover;  // Assign the Rover in the Inspector
    public Vector3 offset = new Vector3(0, 5, -12); // Adjust for better view
    public float smoothSpeed = 6f;

    void LateUpdate()
    {
        if (rover != null)
        {
            Vector3 desiredPosition = rover.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(rover.position); // Ensures the camera faces the rover
        }
    }
}
