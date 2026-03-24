using UnityEngine;

public class SimpleDriftCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offsets")]
    [SerializeField] private Vector3 idleOffset = new Vector3(0, 2, -6f); // offset when car is slow
    [SerializeField] private Vector3 movingOffset = new Vector3(0, 2, -8f); // offset when car is moving

    [Header("Smoothing")]
    [SerializeField] private float positionSmooth = 5f;
    [SerializeField] private float rotationSmooth = 10f;

    [Header("Sway & Tilt")]
    [SerializeField] private float swayAmount = 0.3f; // subtle lean
    [SerializeField] private float tiltAmount = 3f;  // small tilt in drift

    [Header("FOV")]
    [SerializeField] private Camera cam;
    [SerializeField] private float baseFOV = 60f;
    [SerializeField] private float maxFOV = 70f;
    [SerializeField] private float fovSmooth = 2f;

    private Vector3 currentVelocity;

    private void LateUpdate()
    {
        if (target == null) return;

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb == null) return;

        float speed = rb.linearVelocity.magnitude;

        // --- Determine offset based on speed ---
        Vector3 desiredOffset = Vector3.Lerp(idleOffset, movingOffset, Mathf.Clamp01(speed / 30f));

        // Sway based on lateral velocity (subtle)
        Vector3 localVel = target.InverseTransformDirection(rb.linearVelocity);
        float sway = Mathf.Clamp(-localVel.x * swayAmount, -swayAmount, swayAmount);

        Vector3 desiredPosition = target.position
                                  + target.right * sway
                                  + target.up * desiredOffset.y
                                  - target.forward * Mathf.Abs(desiredOffset.z);

        // Smooth position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / positionSmooth);

        // Look at slightly above car
        Vector3 lookPoint = target.position + Vector3.up * 1f;

        // Tilt camera slightly based on lateral velocity
        Quaternion tilt = Quaternion.LookRotation(lookPoint - transform.position) *
                          Quaternion.Euler(0, 0, -localVel.x * tiltAmount);

        transform.rotation = Quaternion.Slerp(transform.rotation, tilt, rotationSmooth * Time.deltaTime);

        // --- FOV adjustment ---
        if (cam != null)
        {
            float targetFOV = Mathf.Lerp(baseFOV, maxFOV, speed / 60f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, fovSmooth * Time.deltaTime);
        }
    }
}