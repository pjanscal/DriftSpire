using UnityEngine;

public class DriftCarCode : CarCode
{
    [Header("Drift Car Settings")] //Instellingen specifiek voor de drift auto 
    [SerializeField] private float driftGripFactor = 0.8f;
    [SerializeField] private float normalGripFactor = 1.0f;
    [SerializeField] private float driftSpeedThreshold = 20f;

    private bool isDrifting = false;

    public override void Steer(float steeringInput)
    {
        base.Steer(steeringInput);

        bool tryingToDrift = currentSpeed > driftSpeedThreshold && Mathf.Abs(steeringInput) > 0.3f &&
                                                                Input.GetAxis("Vertical") > 0.1f;

        if (tryingToDrift && !isDrifting)
        {
            StartDrift();   
        }
       else if (!tryingToDrift && isDrifting)
        {
            StopDrift();
        }

        if (isDrifting) //Drift physics toepassen aan de auto
        {
            ApplyDriftPhysics();
        }
    }

    private void StartDrift()
    {
        isDrifting = true;
        Debug.Log("Starting Drift...");

        AdjustWheelGrip(driftGripFactor); //Grip van de wielen aanpassen om een drift te kunnen houden.

    }

    private void StopDrift()
    {
        isDrifting = false;
        Debug.Log("Stopping Drift...");

        AdjustWheelGrip(normalGripFactor); //Grip van de wielen terug zetten naar normaal.
    }

    private void ApplyDriftPhysics()
    {
        Vector3 lateralVelocity = transform.right * Vector3.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity -= lateralVelocity * 0.3f * Time.fixedDeltaTime;
        //Snelheid lezen en verminderen, laat het auto glijden zonder helemaal uit te glijden.
        //0.3f is een factor voor hoe sterk de drift is.
    }

    private void AdjustWheelGrip(float gripModifier)
    {
        WheelFrictionCurve rearLeftFriction = rearLeftWheel.sidewaysFriction;
        WheelFrictionCurve rearRightFriction = rearRightWheel.sidewaysFriction;

        //Lager stiffness = minder grip, hoger stiffness = meer grip.
        rearLeftFriction.stiffness = 1.0f * gripModifier;
        rearRightFriction.stiffness = 1.0f * gripModifier;

        //Stiffness toepassen.
        rearLeftWheel.sidewaysFriction = rearLeftFriction;
        rearRightWheel.sidewaysFriction = rearRightFriction;

    }
}
