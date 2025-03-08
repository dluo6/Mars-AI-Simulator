using System.Collections;
using UnityEngine;

public class RoverController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBrakeForce;
    private bool isBraking;
    
    // AI & Manual Control Toggle
    [SerializeField] public bool useAI = true;  // Game starts in AI mode

    // Settings
    [SerializeField] private float motorForce;
    [SerializeField] private float brakeForce;
    [SerializeField] private float maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;
    [SerializeField] private WheelCollider middleLeftWheelCollider, middleRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;
    [SerializeField] private Transform middleLeftWheelTransform, middleRightWheelTransform;

    private float baseMotorForce; // Store the base motor force for resetting
    private float adjustedMotorForce; // Temporary adjusted motor force

    private void Start()
    {
        baseMotorForce = motorForce; // Store the initial motor force
        adjustedMotorForce = baseMotorForce; // Initialize adjusted motor force
    }

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
        GetInput(); 
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
            isBraking = Input.GetKey(KeyCode.Space);
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

    // Getter for motor force (so AI can store the base value)
    public float GetMotorForce()
    {
        return baseMotorForce;
    }

    // Setter for motor force (so AI can modify speed based on terrain)
    public void SetMotorForce(float newMotorForce)
    {
        adjustedMotorForce = Mathf.Clamp(newMotorForce, baseMotorForce * 0.25f, baseMotorForce * 5f); 
        // Limits to prevent infinite speed gain/loss
    }

    private void HandleMotor()
    {
        // Use the adjusted motor force for wheel torque
        Debug.Log($"Adjusted Motor Force: {adjustedMotorForce}");
        frontLeftWheelCollider.motorTorque = verticalInput * adjustedMotorForce;
        frontRightWheelCollider.motorTorque = verticalInput * adjustedMotorForce;

        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();
    }

    public void ApplyBraking()
    {
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
        middleLeftWheelCollider.brakeTorque = currentBrakeForce;
        middleRightWheelCollider.brakeTorque = currentBrakeForce;
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