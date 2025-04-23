using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverAIController : MonoBehaviour
{
    private IRoverController roverController;
    private Rigidbody rb;
    private float moveTime;
    private float baseMotorForce;

    // Steering variables
    private float direction = 0;
    private float driveDirection = 1;

    // New water-seeking variables from first script
    private float targetReachedThreshold = 450f;
    private Vector3 currentWaterTarget;
    private bool hasTarget = false;
    private List<Vector3> visitedWaterSources = new List<Vector3>();
    private CheckWetArea checkWetArea;

    [Header("Movement Settings")]
    [SerializeField] private float minMoveTime;
    [SerializeField] private float maxMoveTime;
    [SerializeField] private float turnDuration;
    [SerializeField] private float uphillMultiplier;
    [SerializeField] private float downhillMultiplier;
    [SerializeField] private float steepSlopeThreshold;
    [SerializeField] private float maxDownhillSlope;
    [SerializeField] private float brakeDuration;
    [SerializeField] private float reverseDuration;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private RoverManager roverManager;

    private void Start()
    {
        if (roverManager == null) roverManager = FindAnyObjectByType<RoverManager>();

        if (roverManager != null)
        {
            roverManager.OnRoverChanged += HandleRoverChange;
            if (roverManager != null && roverManager.CurrentRover != null)
            {
                HandleRoverChange(roverManager.CurrentRover);
            }
        }

        checkWetArea = GetComponent<CheckWetArea>(); // From first script
        StartCoroutine(SeekWater()); // From first script
    }


    // Rest of your existing methods...
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

    // New water-seeking coroutine from first script
    private IEnumerator SeekWater()
    {
        while (true)
        {
            if (!roverController.useAI)
            {
                yield return null;
                continue;
            }

            if (!hasTarget)
            {
                currentWaterTarget = FindClosestWaterSource();
                if (currentWaterTarget != Vector3.zero)
                {
                    hasTarget = true;
                    Debug.Log("New water target: " + currentWaterTarget);
                }
                else
                {
                    Debug.Log("All water sources visited");
                    yield break;
                }
            }

            Vector3 toTarget = currentWaterTarget - roverController.transform.position;
            float distanceToTarget = toTarget.magnitude;

            if (distanceToTarget < targetReachedThreshold &&
                checkWetArea != null && checkWetArea.IsOnWetArea)
            {
                visitedWaterSources.Add(currentWaterTarget);
                hasTarget = false;
                yield return new WaitForSeconds(2f);
                continue;
            }

            float angle = Vector3.SignedAngle(roverController.transform.forward, toTarget, Vector3.up);
            direction = Mathf.Clamp(-angle / 45f, -1f, 1f);
            driveDirection = 1f;

            yield return null;
        }
    }

    // New water source finding from first script
    private Vector3 FindClosestWaterSource()
    {
        Vector3 closest = Vector3.zero;
        float minDistance = Mathf.Infinity;

        foreach (Vector3 waterSource in GenerateWetAreas.WaterSources)
        {
            if (visitedWaterSources.Contains(waterSource))
                continue;

            float distance = Vector3.Distance(roverController.transform.position, waterSource);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = waterSource;
            }
        }
        return closest;
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