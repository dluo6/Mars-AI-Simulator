using UnityEngine;


public class Rover1Controller : MonoBehaviour, IRoverController
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBrakeForce;
    private bool isBraking;
    public bool useAI = true;

    // Settings
    private float baseMotorForce;
    private float adjustedMotorForce;
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

    // Rover Body
    public Transform RoverTransform => transform;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1.5f, 0);
    }
    private void Start()
    {
        baseMotorForce = motorForce;
        adjustedMotorForce = baseMotorForce;
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

    // Manual user inputs
    private void GetInput()
    {
        if (!useAI)
        {

            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = -Input.GetAxis("Vertical");
            isBraking = Input.GetKey(KeyCode.Space);
        }
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

    public float GetCurrentSpeed()
    {
        if (rb == null) return 0f;
        return rb.linearVelocity.magnitude;
    }

    bool IRoverController.useAI
    {
        get => useAI;
        set => useAI = value;
    }

}