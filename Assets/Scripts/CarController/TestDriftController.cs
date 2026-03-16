using UnityEngine;

public class TestDriftController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider rearLeft;
    [SerializeField] private WheelCollider rearRight;

    [Header("Settings")]
    [SerializeField] private float motorPower = 1500f;
    [SerializeField] private float brakePower = 3000f;
    [SerializeField] private float steerAngle = 30f;

    [Header("Drift Settings")]
    [SerializeField] private float normalRearGrip = 1.0f;
    [SerializeField] private float driftRearGrip = 0.3f;
    [SerializeField] private float driftEntrySpeed = 10f;
    [SerializeField] private float minDriftAngle = 8f;
    [SerializeField] private float driftExitAngle = 5f;   
    [SerializeField] private float gripTransitionSpeed = 2f; //Hoe snel de grip verandert tussen het driften en niet driften

    [Header("Stability Control")]
    [SerializeField] private float driftStabilization = 0.5f;
    [SerializeField] private float maxStableAngle = 35f;

    [Header("Engine & Speed")]
    [SerializeField] private float maxRPM = 7000f;
    [SerializeField] private float idleRPM = 800f;
    [SerializeField] private AnimationCurve enginePowerCurve;

    [Header("Gear System")]
    [SerializeField] private bool useGears = true;
    [SerializeField] private float[] gearMaxSpeeds = new float[] { 8f, 15f, 23f, 32f, 45f, 60f };

    private Rigidbody rb;
    private bool isDrifting = false;
    private float currentSpeed = 0f;
    private float driftAngle = 0f;
    private float currentGrip = 1.0f;
    private float timeSinceDriftEnd = 0f;

    private int currentGear = 1;
    private float currentRPM = 800f;
    private float wheelRPM = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(-0.2f, 0.17f, 0);
        currentGrip = normalRearGrip;

        if (enginePowerCurve == null || enginePowerCurve.length == 0)
        {
            enginePowerCurve = new AnimationCurve();
            enginePowerCurve.AddKey(0f, 0.4f);      // 0 RPM (eerste f = RPM)
            enginePowerCurve.AddKey(1000f, 0.6f);  
            enginePowerCurve.AddKey(3000f, 0.85f);
            enginePowerCurve.AddKey(5000f, 1.0f); 
            enginePowerCurve.AddKey(7000f, 0.8f);   //Redline, minder kracht omdat de RPM te hoog is
        }
    }

    void FixedUpdate()
    {
        
        float throttle = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        bool handbrake = Input.GetKey(KeyCode.Space);

        //Snelheid en drifthoek berekenen
        currentSpeed = rb.linearVelocity.magnitude;
        UpdateGearSystem();
        driftAngle = CalculateDriftAngle();


        float motor = 0f;
        bool isMovingForward = Vector3.Dot(rb.linearVelocity, transform.forward) > 0.5f;
        bool isMovingBackward = Vector3.Dot(rb.linearVelocity, transform.forward) < -0.5f;
        bool isNearlyStopped = currentSpeed < 0.5f;

        if (throttle > 0.1f)
        {
            if (isMovingBackward && !isNearlyStopped)
            {
                motor = 0f;
                ApplyFullBrake(throttle);
            }
            else
            {
                wheelRPM = (rearLeft.rpm + rearRight.rpm) / 2f;

                float targetRPM = idleRPM;

                if (Mathf.Abs(wheelRPM) > 10f && useGears)
                {
                    float[] gearRatios = new float[] { 12f, 9f, 7f, 5.5f, 4f, 3f }; // RPM keer factor voor elke versnelling.

                    int gearIndex = Mathf.Clamp(currentGear - 1, 0, gearRatios.Length - 1);
                    float gearRatio = gearRatios[gearIndex];

                    //RPM berekenen voor de huidige snelheid+versnellingsstand.
                    targetRPM = Mathf.Abs(wheelRPM) * gearRatio;
                    targetRPM = Mathf.Clamp(targetRPM, idleRPM, maxRPM);
                }

                //Slome overgang naar de target RPM. Zonder dit gaat de RPM gelijk naar de max.
                currentRPM = Mathf.Lerp(currentRPM, targetRPM, Time.fixedDeltaTime * 8f);

                //Power curve voor soepelere rijgedrag.
                float powerMultiplier = enginePowerCurve.Evaluate(currentRPM);

                float gearMultiplier = 1.0f;
                if (useGears)
                {
                    gearMultiplier = (7f - currentGear) / 3f; //Voorbeeld: Gear 1 = 2.0x, Gear 6 = 0.33x
                    gearMultiplier = Mathf.Max(gearMultiplier, 0.5f); // Minimum 0.5x
                }

                motor = throttle * motorPower * powerMultiplier * gearMultiplier;

                // Drift boost
                if (isDrifting && driftAngle > 10f && driftAngle < 40f)
                {
                    motor *= 1.5f; // 50% boost
                }
                else if (isDrifting && driftAngle >= 40f)
                {
                    motor *= 1.2f;
                }
            }
        }
        else if (throttle < -0.1f)
        {
            // S pressed - want to go backward
            if (isMovingForward && !isNearlyStopped)
            {
                // Moving forward but want backward - BRAKE instead
                motor = 0f;
                ApplyFullBrake(-throttle);
            }
            else if (isNearlyStopped)
            {
                // Stopped - can reverse now
                motor = throttle * motorPower * 0.5f; // Half power in reverse
            }
        }
        else
        {
            // No input - coast (motor = 0)
            motor = 0f;
        }

        // Apply the calculated motor torque
        rearLeft.motorTorque = motor;
        rearRight.motorTorque = motor;

        //Drift boost systeem: meer kracht aan het auto geven tijdens het driften (gebasseerd op hoek!)
        if (isDrifting)
        {
            if (driftAngle > 10f && driftAngle < 40f)
            {
                
                motor *= 2.8f;
            }
            else if (driftAngle >= 40f)
            {
                //Hetzelfde idee maar minder boost bij een te grote hoek.
                motor *= 1.2f;
            }
        }

        rearLeft.motorTorque = motor;
        rearRight.motorTorque = motor;

        
        float steering = steer * steerAngle;
        frontLeft.steerAngle = steering;
        frontRight.steerAngle = steering;

        // Drift modus
        UpdateDriftState(steer, handbrake);

        
        if (isDrifting)
        {
            ApplyDriftStabilization();
        }

        // Apply brakes
        if (handbrake)
        {
            rearLeft.brakeTorque = brakePower;
            rearRight.brakeTorque = brakePower;
            frontLeft.brakeTorque = 0;
            frontRight.brakeTorque = 0;
        }
        else if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.1f)
        {
            // Only release brakes if no input (don't override ApplyFullBrake)
            frontLeft.brakeTorque = 0;
            frontRight.brakeTorque = 0;
            rearLeft.brakeTorque = 0;
            rearRight.brakeTorque = 0;
        }

        if (!isDrifting && timeSinceDriftEnd < 2f) //Motor boost voor X aantal seconden na het driften.
        {
            motor *= 1.3f;
            timeSinceDriftEnd += Time.fixedDeltaTime;
        }

        if (isDrifting)
        {
            timeSinceDriftEnd = 0f;
        }

        // Smooth grip transition
        float targetGrip = isDrifting ? driftRearGrip : normalRearGrip;
        currentGrip = Mathf.Lerp(currentGrip, targetGrip, Time.fixedDeltaTime * gripTransitionSpeed);
        ApplyRearGrip(currentGrip);

        // Manual drift toggle
        if (Input.GetKeyDown(KeyCode.D))
        {
            isDrifting = !isDrifting;
            Debug.Log("Manual drift toggle: " + isDrifting);
        }
    }

    //Drift stabilisatie systeem om te voorkomen dat de auto hopeloos spint.
    private void ApplyDriftStabilization()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);

        float lateralVelocity = localVelocity.x;

        float stabilizationAmount = driftStabilization;

        if (driftAngle > maxStableAngle)
        {
            float overAngle = (driftAngle - maxStableAngle) / 20f; 
            stabilizationAmount = Mathf.Lerp(driftStabilization, 0.9f, overAngle);
        }

        
        localVelocity.x *= (1f - stabilizationAmount * Time.fixedDeltaTime * 10f);

        
        rb.linearVelocity = transform.TransformDirection(localVelocity);
        rb.angularVelocity *= (1f - stabilizationAmount * Time.fixedDeltaTime * 5f);
    }

    private void UpdateGearSystem()
    {
        if (!useGears) return;

        //Naar boven schakelen als RPM te hoog is.
        if (currentRPM > maxRPM * 0.85f && currentGear < gearMaxSpeeds.Length)
        {
            currentGear++;
            Debug.Log("Shifted UP to gear " + currentGear + " (RPM was " + currentRPM.ToString("F0") + ")");
        }
        //Naar beneden schakelen als RPM te laag is.
        else if (currentRPM < maxRPM * 0.3f && currentGear > 1 && currentSpeed > 5f)
        {
            currentGear--;
            Debug.Log("Shifted DOWN to gear " + currentGear + " (RPM was " + currentRPM.ToString("F0") + ")");
        }
    }

    private void UpdateDriftState(float steer, bool handbrake)
    {
        //Condities voor DRIFT STARTEN.
        bool tryingToDrift = handbrake || Mathf.Abs(steer) > 0.4f;
        bool fastEnough = currentSpeed > driftEntrySpeed;
        bool canEnterDrift = tryingToDrift && fastEnough;

        //Condities voor IN DRIFT blijven.
        bool stillSliding = driftAngle > driftExitAngle;
        bool stillSteering = Mathf.Abs(steer) > 0.1f;
        bool shouldStayInDrift = stillSliding || stillSteering || handbrake;

        if (!isDrifting && canEnterDrift)
        {
            //Drift starten.
            isDrifting = true;
            Debug.Log(">>> DRIFT START - Speed: " + currentSpeed.ToString("F1"));
        }
        else if (isDrifting && !shouldStayInDrift)
        {
            //Drift einde.
            isDrifting = false;
            Debug.Log("<<< DRIFT END - Angle: " + driftAngle.ToString("F1"));
        }

        //Eindig drift als de auto de sloom gaat.
        if (currentSpeed < 3f)
        {
            isDrifting = false;
        }
    }

    private void ApplyFullBrake(float brakeInput)
    {
        float brake = brakeInput * brakePower;
        frontLeft.brakeTorque = brake;
        frontRight.brakeTorque = brake;
        rearLeft.brakeTorque = brake;
        rearRight.brakeTorque = brake;
    }

    private float CalculateDriftAngle()
    {
        if (currentSpeed < 2f) return 0f;

        Vector3 forward = transform.forward;

        //Echte snelheids richting.
        Vector3 velocity = rb.linearVelocity.normalized;

        //Hoek tussen waar de auto naartoe kijkt en waar die naartoe gaat. (pos. = rechts, neg.= links)
        float angle = Vector3.SignedAngle(forward, velocity, Vector3.up);

        return Mathf.Abs(angle);
    }

    private void ApplyRearGrip(float grip)
    {
        WheelFrictionCurve rearLeftSideways = rearLeft.sidewaysFriction;
        WheelFrictionCurve rearRightSideways = rearRight.sidewaysFriction;

        rearLeftSideways.stiffness = grip;
        rearRightSideways.stiffness = grip;

        rearLeft.sidewaysFriction = rearLeftSideways;
        rearRight.sidewaysFriction = rearRightSideways;

        WheelFrictionCurve rearLeftForward = rearLeft.forwardFriction;
        WheelFrictionCurve rearRightForward = rearRight.forwardFriction;

        rearLeftForward.stiffness = grip;
        rearRightForward.stiffness = grip;

        rearLeft.forwardFriction = rearLeftForward;
        rearRight.forwardFriction = rearRightForward;
    }

    private void OnGUI()
    {
        int y = 10;

        GUI.Label(new Rect(10, y, 300, 20), "Speed: " + currentSpeed.ToString("F1") + " m/s");
        y += 20;

        GUI.Label(new Rect(10, y, 300, 20), "Drift Angle: " + driftAngle.ToString("F1") + "°");
        y += 20;

        GUI.Label(new Rect(10, y, 300, 20), "Current Grip: " + currentGrip.ToString("F2"));
        y += 20;

        GUI.Label(new Rect(10, y, 300, 20), "Drift Mode: " + isDrifting);
        y += 20;

        if (isDrifting)
        {
            GUI.color = Color.yellow;
            GUI.Label(new Rect(10, y, 300, 30), "DRIFTING");
            y += 30;
        }

        GUI.color = Color.white;
        GUI.Label(new Rect(10, y, 400, 20), "WASD + Space (handbrake) | D = force toggle");
        y += 20;

        GUI.color = Color.cyan;
        GUI.Label(new Rect(10, y, 500, 20), "Tip: Handbrake + turn to START, keep steering to HOLD");

        // Add RPM display
        GUI.color = currentRPM > maxRPM * 0.9f ? Color.red : Color.white;
        GUI.Label(new Rect(10, y, 300, 20), "RPM: " + currentRPM.ToString("F0") + " / " + maxRPM.ToString("F0"));
        y += 20;
        GUI.color = Color.white;

        // Then your existing speed line
        GUI.Label(new Rect(10, y, 300, 20), "Speed: " + currentSpeed.ToString("F1") + " m/s (" + (currentSpeed * 3.6f).ToString("F0") + " km/h)");
        y += 20;

        if (useGears)
        {
            GUI.Label(new Rect(10, y, 300, 20), "Gear: " + currentGear + " / " + gearMaxSpeeds.Length);
            y += 20;
        }
    }
}