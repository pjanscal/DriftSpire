using UnityEngine;

public class DriftControllerFixed : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeft, frontRight, rearLeft, rearRight;

    [Header("Car Settings")]
    public float motorPower = 1800f;
    public float brakePower = 4000f;
    public float steerAngle = 35f;

    [Header("Drift Settings")]
    public float normalGrip = 1.2f;
    public float driftGrip = 0.4f;
    public float gripLerpSpeed = 6f;
    public float driftMinSpeed = 8f;

    [Header("Stability")]
    public float angularDamping = 2f;

    [Header("Gears")]
    public bool useGears = true;
    public float[] gearMultipliers = { 4f, 3f, 2f, 1.5f, 1f, 0.7f };
    public float[] gearMaxSpeeds = { 12f, 20f, 28f, 35f, 45f, 60f }; // m/s per gear
    public float maxReverseSpeed = 8f; // m/s for reverse

    private int currentGear = 0; // -1 for reverse
    private Rigidbody rb;
    private bool isDrifting;
    private float currentGrip;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentGrip = normalGrip;
    }

    void Update()
    {
        // --- MANUAL GEAR SHIFT ---
        if (useGears)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (currentGear == -1) currentGear = 0; // leave reverse
                else currentGear = Mathf.Min(currentGear + 1, gearMultipliers.Length - 1);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (currentGear > 0) currentGear--;
                else if (currentGear == 0) currentGear = -1; // allow reverse
            }
        }

        // --- RESET CAR ---
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCar();
        }
    }

    void FixedUpdate()
    {
        float throttle = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        bool handbrake = Input.GetKey(KeyCode.Space);

        // Forward speed along car's facing direction
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        // --- STEERING ---
        float steerInput = steer * steerAngle;
        frontLeft.steerAngle = steerInput;
        frontRight.steerAngle = steerInput;

        // --- DRIFT STATE ---
        isDrifting = (handbrake || Mathf.Abs(steer) > 0.5f) && rb.linearVelocity.magnitude > driftMinSpeed;

        // --- MOTOR ---
        float motor = throttle * motorPower;

        // Reverse
        if (currentGear == -1 || (throttle < 0f && forwardSpeed <= 0f && currentGear == 0))
        {
            // Reverse gear torque
            motor = throttle * motorPower;
            if (Mathf.Abs(forwardSpeed) > maxReverseSpeed)
            {
                motor = 0f; // clamp reverse speed
            }
        }
        else if (useGears)
        {
            // Forward gears
            motor *= gearMultipliers[currentGear];

            float maxSpeedThisGear = gearMaxSpeeds[currentGear];
            if (forwardSpeed > maxSpeedThisGear)
            {
                motor = 0f; // clamp per-gear speed
            }
        }

        // Drift boost
        if (isDrifting)
        {
            motor *= 1.2f;
        }

        rearLeft.motorTorque = motor;
        rearRight.motorTorque = motor;

        // --- BRAKES ---
        float brake = 0f;

        if (throttle < 0f && forwardSpeed > 0f)
        {
            brake = -throttle * brakePower;
        }

        if (handbrake)
        {
            // Lock rear wheels
            rearLeft.brakeTorque = Mathf.Infinity;
            rearRight.brakeTorque = Mathf.Infinity;
        }
        else
        {
            rearLeft.brakeTorque = brake;
            rearRight.brakeTorque = brake;
        }

        frontLeft.brakeTorque = brake;
        frontRight.brakeTorque = brake;

        // --- GRIP ---
        float targetGrip = isDrifting ? driftGrip : normalGrip;
        currentGrip = Mathf.Lerp(currentGrip, targetGrip, Time.fixedDeltaTime * gripLerpSpeed);
        ApplyGrip(currentGrip);

        // --- ASSISTS ---
        //ApplyCounterSteer(steer); // still commented out
        ApplyWeightTransfer(steer);

        // --- DRIFT BOOST ---
        if (isDrifting && rb.linearVelocity.magnitude > 10f)
        {
            rb.AddForce(transform.forward * 300f);
        }

        // --- STABILITY ---
        rb.angularVelocity *= (1f - Time.fixedDeltaTime * angularDamping);

        // Anti-roll torque
        float rollAngle = Vector3.SignedAngle(Vector3.up, transform.up, transform.forward);
        float antiRoll = -rollAngle * 5f;
        rb.AddTorque(transform.forward * antiRoll, ForceMode.Acceleration);
    }

    void ResetCar()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position += Vector3.up * 1.5f;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    void ApplyGrip(float grip)
    {
        WheelFrictionCurve rearSide = rearLeft.sidewaysFriction;
        rearSide.stiffness = grip;
        rearLeft.sidewaysFriction = rearSide;
        rearRight.sidewaysFriction = rearSide;

        WheelFrictionCurve rearForward = rearLeft.forwardFriction;
        rearForward.stiffness = normalGrip;
        rearLeft.forwardFriction = rearForward;
        rearRight.forwardFriction = rearForward;

        WheelFrictionCurve frontSide = frontLeft.sidewaysFriction;
        frontSide.stiffness = normalGrip * 1.2f;
        frontLeft.sidewaysFriction = frontSide;
        frontRight.sidewaysFriction = frontSide;
    }

    //void ApplyCounterSteer(float steerInput)
    //{
    //    Vector3 localVel = transform.InverseTransformDirection(rb.velocity);
    //    float sideways = localVel.x;
    //
    //    if (Mathf.Abs(sideways) > 2f)
    //    {
    //        float assist = sideways * 0.1f;
    //        float finalSteer = steerInput + assist;
    //
    //        frontLeft.steerAngle = finalSteer * steerAngle;
    //        frontRight.steerAngle = finalSteer * steerAngle;
    //    }
    //}

    void ApplyWeightTransfer(float steer)
    {
        float transfer = steer * rb.mass * 0.02f;
        rb.AddForce(transform.right * -transfer, ForceMode.Force);
    }
}