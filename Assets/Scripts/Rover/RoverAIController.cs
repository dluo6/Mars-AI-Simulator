using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverAIController : MonoBehaviour
{
    private RoverController roverController;
    private Rigidbody rb;
    private float baseMotorForce;
    
    // Steering variables
    private float direction = 0; 
    private float driveDirection = 1;
    private float targetReachedThreshold = 450f;

    [SerializeField] private float turnDuration;
    [SerializeField] private float brakeDuration; 
    [SerializeField] private float reverseDuration; 
    [SerializeField] private float raycastDistance; 
    [SerializeField] private LayerMask groundLayer;

    private Vector3 currentWaterTarget;
    private bool hasTarget = false;
    private List<Vector3> visitedWaterSources = new List<Vector3>();

    private CheckWetArea checkWetArea;

    private void Start()
    {
        roverController = GetComponent<RoverController>();
        checkWetArea = GetComponent<CheckWetArea>();  
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -1.5f, 0);
        rb = GetComponent<Rigidbody>();
        baseMotorForce = roverController.GetMotorForce();
        
        // Start the water-seeking coroutine.
        StartCoroutine(SeekWater());
    }

    private void FixedUpdate()
    {
        if (roverController.useAI)
        {
            AdjustSpeedBasedOnSlope();
            roverController.SetInputs(direction, driveDirection);

            if (IsSteepDownhill())
            {
                StartCoroutine(BrakeAndReverse());
            }
        }
    }

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
                    Debug.Log("All water sources have been visited or none available.");
                    yield break;
                }
            }

            // Calculate vector to the target water source.
            Vector3 toTarget = currentWaterTarget - transform.position;
            float distanceToTarget = toTarget.magnitude;

            // Check if the rover is near the target AND on a wet area.
            if (distanceToTarget < targetReachedThreshold && 
                checkWetArea != null && checkWetArea.IsOnWetArea)
            {
                Debug.Log("Reached target water source: " + currentWaterTarget);
                visitedWaterSources.Add(currentWaterTarget);
                hasTarget = false;
                // Optionally wait before searching for the next target.
                yield return new WaitForSeconds(2f);
                continue;
            }

            // If not yet reached, steer toward the target.
            float angle = Vector3.SignedAngle(transform.forward, toTarget, Vector3.up);
            direction = Mathf.Clamp(-angle / 45f, -1f, 1f);
            driveDirection = 1f;  // Always drive forward when seeking water.
            
            yield return null;
        }
    }


    // finding the closest water source that hasn’t been visited.
    private Vector3 FindClosestWaterSource()
    {
        Vector3 closest = Vector3.zero;
        float minDistance = Mathf.Infinity;

        foreach (Vector3 waterSource in GenerateWetAreas.WaterSources)
        {
            if (visitedWaterSources.Contains(waterSource))
                continue;

            float distance = Vector3.Distance(transform.position, waterSource);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = waterSource;
            }
        }
        return closest;
    }

    private bool IsSteepDownhill()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position + transform.forward * -30f + Vector3.up * 1f;
        Vector3 raycastDirection = Vector3.down; 
        Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.blue);

        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastDistance, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return slopeAngle > 30f; 
        }
        return false;
    }

    private void AdjustSpeedBasedOnSlope()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position + Vector3.up * 1f;
        Vector3 raycastDirection = Vector3.down;
        Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.red);

        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastDistance, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            float roverPitchAngle = Vector3.Angle(transform.forward, Vector3.up) - 90f;

            if (slopeAngle > 10f) 
            {
                if (roverPitchAngle > 10f)
                {
                    roverController.SetMotorForce(baseMotorForce * 2.5f);
                    // Debug.Log("Uphill! Increased motor force.");
                }
                else if (roverPitchAngle < -5f)
                {
                    roverController.SetMotorForce(baseMotorForce * 0.5f);
                    // Debug.Log("Downhill! Reduced motor force.");
                }
                else
                {
                    roverController.SetMotorForce(baseMotorForce);
                    // Debug.Log("Level terrain. Normal motor force.");
                }
            }
            else
            {
                roverController.SetMotorForce(baseMotorForce);
                // Debug.Log("Level terrain. Normal motor force.");
            }

            if (slopeAngle > 30f && roverPitchAngle < -5f)
            {
                StartCoroutine(BrakeAndReverse());
            }
        }
        else
        {
            roverController.SetMotorForce(baseMotorForce);
        }
    }

    private IEnumerator BrakeAndReverse()
    {
        roverController.ApplyBraking();
        yield return new WaitForSeconds(brakeDuration);
        roverController.SetInputs(0, -1);
        yield return new WaitForSeconds(reverseDuration);
        direction = Random.value > 0.5f ? 1f : -1f;
        yield return new WaitForSeconds(turnDuration);
        direction = 0;
    }
}
