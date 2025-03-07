using System.Collections;
using UnityEngine;

public class RoverController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;
    private bool isBreaking;
    
    // AI & Manual Control Toggle
    [SerializeField] public bool useAI = true;  // Game starts in AI mode

    // Settings
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;
    [SerializeField] private WheelCollider middleLeftWheelCollider, middleRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;
    [SerializeField] private Transform middleLeftWheelTransform, middleRightWheelTransform;

    private void Update()
    {
        // Press "M" to toggle AI & Manual Control
        if (Input.GetKeyDown(KeyCode.M))
        {
            useAI = !useAI;
            Debug.Log(useAI ? "AI Control Enabled" : "Manual Control Enabled");
        }
    }

    private void FixedUpdate()
    {
        GetInput();   // AI or manual control logic
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        if (!useAI)
        {
            // Manual Steering Input
            horizontalInput = Input.GetAxis("Horizontal");
            
            // Manual Acceleration Input
            verticalInput = -Input.GetAxis("Vertical");

            // Braking Input
            isBreaking = Input.GetKey(KeyCode.Space);
        }
        // AI will set `horizontalInput` and `verticalInput` externally via `SetInputs()`
    }

    // Allow AI to set movement inputs
    public void SetInputs(float horizontal, float vertical)
    {
        if (useAI)
        {
            horizontalInput = horizontal;
            verticalInput = vertical;
        }
    }

    // Adjust speed dynamically based on terrain slope
    public void AdjustSpeed(float multiplier)
    {
        motorForce *= multiplier;
    }

    // Getter for motor force (so AI can store the base value)
    public float GetMotorForce()
    {
        return motorForce;
    }   

    // Setter for motor force (so AI can modify speed based on terrain)
    public void SetMotorForce(float newMotorForce)
    {
        motorForce = Mathf.Clamp(newMotorForce, GetMotorForce() * 0.5f, GetMotorForce() * 1.5f); 
        // Limits to prevent infinite speed gain/loss
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        
        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    public void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
        middleLeftWheelCollider.brakeTorque = currentBreakForce;
        middleRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(middleRightWheelCollider, middleRightWheelTransform);
        UpdateSingleWheel(middleLeftWheelCollider, middleLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
