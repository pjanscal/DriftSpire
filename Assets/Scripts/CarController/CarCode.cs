using UnityEngine;

public class CarCode : VehicleCode
{
    [Header("Wheel Colliders")] //Simulatie van de wielen
    [SerializeField] protected WheelCollider frontLeftWheel;
    [SerializeField] protected WheelCollider frontRightWheel;
    [SerializeField] protected WheelCollider rearLeftWheel;
    [SerializeField] protected WheelCollider rearRightWheel;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.mass = 1500;
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    //FixedUpdate is voor de physics, please gebruik dit inplaats van Update, zodat physics niet op framerate afhankelijk zijn.
    private void FixedUpdate()
    {
        //Player input ophalen
        float accelerationInput = Input.GetAxis("Vertical"); //W/S - Up/Down
        float steeringInput = Input.GetAxis("Horizontal"); //A/D - Left/Right

        Accelerate(accelerationInput);
        Steer(steeringInput);

        if (Input.GetKey(KeyCode.Space))
        {
            Brake(1f); //100% remmen als de spatiebalk ingedrukt is
        }
    }

    //Overgeschreven methodes van VehicleCode, echte physics van de auto worden hier toegapast.
    public override void Accelerate(float input)
    {
        //Kracht toepassen op de achterwielen. Dit mag verandered worden als het driften meer controle nodig heeft.
        rearLeftWheel.motorTorque = input * accelerationForce;
        rearRightWheel.motorTorque = input * accelerationForce;

        currentSpeed = rb.linearVelocity.magnitude * 3.6f;
    }

    public override void Brake(float input)
    {
        //Remkracht toepassen op alle wielen.
        frontLeftWheel.brakeTorque = input * brakingForce;
        frontRightWheel.brakeTorque = input * brakingForce;
        rearLeftWheel.brakeTorque = input * brakingForce;
        rearRightWheel.brakeTorque = input * brakingForce;
    }

    public override void Steer(float input)
    {
        //Voorwielen sturen
        float currentSteerAngle = steerAngle * input;
        frontLeftWheel.steerAngle = currentSteerAngle;
        frontRightWheel.steerAngle = currentSteerAngle;
    }
}
