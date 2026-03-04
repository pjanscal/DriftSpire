using UnityEngine;

public abstract class VehicleCode : MonoBehaviour
{
    //Simpele physics data voor de auto

    [Header("Speed Settings")]
    [SerializeField] protected float maxSpeed = 100f;
    [SerializeField] protected float currentSpeed = 0f;

    //Deze variabelen mogen gewijzigd worden voor soepelere/leukere/moeilijkere driften.
    //Als het moet, kunnen wij meer krachten toevoegen als wij diepere besturing willen.
    [Header("Movement Forces")]
    [SerializeField] protected float accelerationForce = 500f;
    [SerializeField] protected float brakingForce = 300f;
    [SerializeField] protected float steerAngle = 30f;

    protected Rigidbody rb;

    //=====Methodes=====

    public virtual void Accelerate(float input) //Input zal tussen 0 en 1 zijn.
        //Placeholder, Child Objecten hebben hun eigen, echte physics.
    {
        Debug.Log("Accelerating...");
    }

    public virtual void Brake(float input) //Input zal tussen 0 en 1 zijn.
    {
        Debug.Log("Braking...");
    }

    public virtual void Steer(float input) //Input zal tussen -1 en 1 moeten zijn.
    {
        Debug.Log("Steering...");
    }
}

//Sommige lijnen zijn "protected". Handig zodat random scripts niet zich ermee bemoeien, alleen de Child objecten.
