using System.Collections;
using UnityEngine;

public class RoverAIController : MonoBehaviour
{
    private RoverController roverController;
    private Rigidbody rb;
    private float moveTime;
    private bool isTurning;
    
    [SerializeField] private float minMoveTime = 10f;
    [SerializeField] private float maxMoveTime = 20f;
    [SerializeField] private float turnDuration = 0.5f;
    [SerializeField] private float uphillMultiplier = 1.7f;  // Boost when climbing
    [SerializeField] private float downhillMultiplier = 0.4f; // Slow down when descending
    [SerializeField] private float steepSlopeThreshold = 20f;
    private float direction = 0; // Turning direction
    private float driveDirection = 1; // Forward (1) or backward (-1)
    private float baseMotorForce; // Stores original motor force

    private void Start()
    {
        roverController = GetComponent<RoverController>();
        rb = GetComponent<Rigidbody>();
        baseMotorForce = roverController.GetMotorForce(); 
        StartCoroutine(Roam());
    }

    private void FixedUpdate()
    {
        if (roverController.useAI)
        {
            AdjustSpeedBasedOnSlope();
            roverController.SetInputs(direction, driveDirection); 
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
                driveDirection = 1.0f;

                yield return new WaitForSeconds(moveTime);

                // Only turn if on a safe, non-steep surface
                if (!IsOnSteepSlope())
                {
                    isTurning = true;
                    direction = Random.value > 0.5f ? 1f : -1f; // Left or right
                    yield return new WaitForSeconds(turnDuration);
                    direction = 0; // Stop turning
                    isTurning = false;
                }
            }
            yield return null;
        }
    }

    private void AdjustSpeedBasedOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
        {
            Vector3 normal = hit.normal;
            float angle = Vector3.Angle(Vector3.up, normal);

            float slopeFactor = angle / 45f; 
            float dot = Vector3.Dot(transform.forward * driveDirection, normal);

            if (dot > 0.2f) // Going uphill
            {
                roverController.SetMotorForce(baseMotorForce * (1 + (slopeFactor * (uphillMultiplier - 1))));
            }
            else if (dot < -0.2f) // Going downhill
            {
                roverController.SetMotorForce(baseMotorForce * (1 - (slopeFactor * (1 - downhillMultiplier))));
            }
            else
            {
                roverController.SetMotorForce(baseMotorForce);
            }
        }
    }

    private bool IsOnSteepSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            return angle > steepSlopeThreshold; 
        }
        return false;
    }
}
