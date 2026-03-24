using UnityEngine;

public class WheelVisuals : MonoBehaviour
{
    public WheelCollider frontLeft, frontRight, rearLeft, rearRight;
    public Transform frontLeftMesh, frontRightMesh, rearLeftMesh, rearRightMesh;

    public float steerSmooth = 8f;

    private float currentSteer;

    void Update()
    {
        UpdateWheel(frontLeft, frontLeftMesh);
        UpdateWheel(frontRight, frontRightMesh);
        UpdateWheel(rearLeft, rearLeftMesh);
        UpdateWheel(rearRight, rearRightMesh);

        SmoothSteering();
    }

    void UpdateWheel(WheelCollider col, Transform mesh)
    {
        if (mesh == null) return;

        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);

        mesh.position = pos;
        mesh.rotation = rot;
    }

    void SmoothSteering()
    {
        float target = frontLeft.steerAngle;
        currentSteer = Mathf.Lerp(currentSteer, target, Time.deltaTime * steerSmooth);

        Vector3 angles = frontLeftMesh.localEulerAngles;
        frontLeftMesh.localEulerAngles = new Vector3(angles.x, currentSteer, angles.z);

        angles = frontRightMesh.localEulerAngles;
        frontRightMesh.localEulerAngles = new Vector3(angles.x, currentSteer, angles.z);
    }
}