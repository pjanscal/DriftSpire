using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //Camera instellingen. Deze mogen via de inspector aangepast worden.
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Positioning")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -6);

    [Header("Smoothing")]
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.LookAt(target.position + Vector3.up * 1.0f);
    }
}
