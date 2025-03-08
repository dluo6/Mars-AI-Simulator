using System.Collections;
using UnityEngine;

public class RoverAIController : MonoBehaviour
{
    private RoverController roverController;
    private Rigidbody rb;
    private float moveTime;
    private bool isTurning;
    
    [SerializeField] private float minMoveTime = 5f;
    [SerializeField] private float maxMoveTime = 15f;
    [SerializeField] private float turnDuration = 2f;
    [SerializeField] private float uphillMultiplier = 3.5f;
    [SerializeField] private float downhillMultiplier = 0.5f;
    [SerializeField] private float steepSlopeThreshold = 20f; // Prevent turns on steep slopes
    [SerializeField] private float maxDownhillSlope = 30f; // Avoid downhill slopes steeper than 30 degrees
    [SerializeField] private float brakeDuration = 4f; // Time to brake before reversing
    [SerializeField] private float reverseDuration = 2f; // Time to reverse before turning

    private float direction = 0; 
    private float driveDirection = 1;
    private float baseMotorForce;

    [SerializeField] private float raycastDistance = 25f; // Distance to detect slope
    [SerializeField] private LayerMask groundLayer; // Layer mask for ground

    private void Start()
    {
        roverController = GetComponent<RoverController>();
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -1.5f, 0); // Lower the center
        rb = GetComponent<Rigidbody>();
        baseMotorForce = roverController.GetMotorForce();
        StartCoroutine(Roam());
    }

    private void FixedUpdate()
    {
        if (roverController.useAI)
        {
            // Always adjust speed based on slope
            AdjustSpeedBasedOnSlope();

            // If not turning, move forward or backward
            if (!isTurning)
            {
                roverController.SetInputs(direction, driveDirection);
            }

            // Check for steep downhill and avoid it
            if (IsSteepDownhill())
            {
                StartCoroutine(BrakeAndReverse());
            }
        }
    }

    private IEnumerator Roam()
    {
        while (true)
        {
            if (roverController.useAI)
            {
                moveTime = Random.Range(minMoveTime, maxMoveTime);
                direction = 0; 

                // Randomly decide to go forward or backward
                // driveDirection = Random.value > 0.5f ? 1f : -1f;

                yield return new WaitForSeconds(moveTime);

                // Randomly decide to turn left or right
                isTurning = true;
                direction = Random.value > 0.5f ? 1f : -1f;
                yield return new WaitForSeconds(turnDuration);

                direction = 0; // Stop turning
                isTurning = false;
            }
            yield return null;
        }
    }

    private bool IsSteepDownhill()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, raycastDistance, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return slopeAngle > maxDownhillSlope;
        }
        return false;
    }

    private void AdjustSpeedBasedOnSlope()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position + transform.up * 0.5f; // Slightly above the rover
        Vector3 raycastDirection = transform.forward; // Raycast in the direction the rover is facing

        Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.red); // Debug raycast

        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastDistance, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            Debug.Log($"Slope Angle: {slopeAngle}"); // Log the slope angle for debugging

            if (slopeAngle > steepSlopeThreshold)
            {
                // Uphill
                roverController.SetMotorForce(baseMotorForce * uphillMultiplier);
                Debug.Log($"Uphill: {baseMotorForce * uphillMultiplier}");
            }
            else if (slopeAngle < -steepSlopeThreshold)
            {
                // Downhill
                roverController.SetMotorForce(baseMotorForce * downhillMultiplier);
                Debug.Log($"Downhill: {baseMotorForce * downhillMultiplier}");
            }
            else
            {
                // Flat terrain
                roverController.SetMotorForce(baseMotorForce);
                Debug.Log($"Level: {baseMotorForce}");
            }
        }
        else
        {
            // No slope detected (flat terrain)
            roverController.SetMotorForce(baseMotorForce);
        }
    }

    private IEnumerator BrakeAndReverse()
    {
        roverController.ApplyBraking(); // Engage brakes
        yield return new WaitForSeconds(brakeDuration);

        roverController.SetInputs(0, -1); // Reverse
        yield return new WaitForSeconds(reverseDuration);

        isTurning = true;
        direction = Random.value > 0.5f ? 1f : -1f; // Turn left or right
        yield return new WaitForSeconds(turnDuration);

        direction = 0; // Resume normal movement
        isTurning = false;
    }
}