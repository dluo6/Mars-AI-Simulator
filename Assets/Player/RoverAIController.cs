using System.Collections;
using UnityEngine;

public class RoverAIController : MonoBehaviour
{
    private IRoverController roverController;

    private Rigidbody rb;
    private float moveTime;
    // private bool isTurning;

    [SerializeField] private float minMoveTime;
    [SerializeField] private float maxMoveTime;
    [SerializeField] private float turnDuration;
    [SerializeField] private float uphillMultiplier;
    [SerializeField] private float downhillMultiplier;
    [SerializeField] private float steepSlopeThreshold; // Prevent turns on steep slopes
    [SerializeField] private float maxDownhillSlope; // Avoid downhill slopes steeper than 30 degrees
    [SerializeField] private float brakeDuration; // Time to brake before reversing
    [SerializeField] private float reverseDuration; // Time to reverse before turning

    private float direction = 0;
    private float driveDirection = 1;
    private float baseMotorForce;

    [SerializeField] private float raycastDistance; // Distance to detect slope
    [SerializeField] private LayerMask groundLayer; // Layer mask for ground

    [SerializeField] private RoverManager roverManager;


    private void Start()
    {
        // Initialize from RoverManager instead of GetComponent
        if (roverManager == null) roverManager = FindAnyObjectByType<RoverManager>();

        if (roverManager != null)
        {
            roverManager.OnRoverChanged += HandleRoverChange;
            InitializeForCurrentrover();
        }
    }

    private void HandleRoverChange(GameObject newRover)
    {
        roverController = newRover.GetComponent<IRoverController>();
        rb = newRover.GetComponent<Rigidbody>();


        if (rb != null)
        {
            rb.centerOfMass = new Vector3(0, -1.5f, 0);
        }

        baseMotorForce = roverController.GetMotorForce();

    }

    private void InitializeForCurrentrover()
    {
        if (roverManager != null && roverManager.CurrentRover != null)
        {
            HandleRoverChange(roverManager.CurrentRover);
        }
    }

    private void FixedUpdate()
    {
        if (roverController.useAI)
        {
            // Always adjust speed based on slope
            AdjustSpeedBasedOnSlope();

            roverController.SetInputs(direction, driveDirection);

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
                driveDirection = Random.value > 0.5f ? 1f : -1f;

                yield return new WaitForSeconds(moveTime);

                // Randomly decide to turn left or right
                // isTurning = true;
                direction = Random.value > 0.5f ? 1f : -1f;
                yield return new WaitForSeconds(turnDuration);

                direction = 0; // Stop turning
                // isTurning = false;
            }
            yield return null;
        }
    }

    private bool IsSteepDownhill()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = new Vector3(roverController.transform.position.x, roverController.transform.position.y + 1f, roverController.transform.position.z - 1f);
        Vector3 raycastDirection = Vector3.down;

        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastDistance, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return slopeAngle > maxDownhillSlope;
        }
        return false;
    }

    private void AdjustSpeedBasedOnSlope()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = roverController.transform.position + Vector3.up * 1f; // Raycast from the center of the rover
        Vector3 raycastDirection = Vector3.down;

        Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.red);

        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastDistance, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            // Calculate the angle between the rover's forward direction and the global "up" vector
            float roverPitchAngle = Vector3.Angle(roverController.transform.forward, Vector3.up) - 90f; // Subtract 90 to get pitch relative to horizontal

            Debug.Log($"Slope Angle: {slopeAngle}, Rover Pitch Angle: {roverPitchAngle}");

            if (slopeAngle > 10f)
            {
                if (roverPitchAngle < -5f) // Downhill (rover is pointing downwards)
                {
                    roverController.SetMotorForce(baseMotorForce * downhillMultiplier);
                    Debug.Log($"Downhill! Motor Force: {baseMotorForce * downhillMultiplier}");
                }
                else if (roverPitchAngle > 10f) // Uphill (rover is pointing upwards)
                {
                    roverController.SetMotorForce(baseMotorForce * uphillMultiplier);
                    Debug.Log($"Uphill! Motor Force: {baseMotorForce * uphillMultiplier}");
                }
                else // Flat or negligible slope
                {
                    roverController.SetMotorForce(baseMotorForce);
                    Debug.Log($"Level Terrain. Motor Force: {baseMotorForce}");
                }
            }
            else // Flat terrain
            {
                roverController.SetMotorForce(baseMotorForce);
                Debug.Log($"Level Terrain. Motor Force: {baseMotorForce}");
            }

            // Check for steep downhill slopes
            if (slopeAngle > maxDownhillSlope && roverPitchAngle < -5f)
            {
                StartCoroutine(BrakeAndReverse());
            }
        }
        else
        {
            // If no ground is detected, assume flat terrain
            roverController.SetMotorForce(baseMotorForce);
            //Debug.LogWarning("No ground detected. Assuming flat terrain.");
        }
    }

    private IEnumerator BrakeAndReverse()
    {
        roverController.ApplyBraking();
        yield return new WaitForSeconds(brakeDuration);

        roverController.SetInputs(0, 1); // Reverse
        yield return new WaitForSeconds(reverseDuration);

        // isTurning = true;
        direction = Random.value > 0.5f ? 1f : -1f; // Turn left or right
        yield return new WaitForSeconds(turnDuration);

        direction = 0; // Resume normal movement
        // isTurning = false;
    }
}