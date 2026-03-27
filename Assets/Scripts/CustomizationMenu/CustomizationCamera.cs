using UnityEngine;

public class CustomizationCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform carTarget;

    [Header("Camera Poses")]
    public Vector3 defaultPosition = new Vector3(15.9f, 0.36f, 4.93f);
    public Vector3 defaultRotation = new Vector3(-3.13f, -107.54f, 0f);

    public Vector3 colorPosition = new Vector3(15.9f, 1.01f, 5.64f);
    public Vector3 colorRotation = new Vector3(4.54f, -105.05f, 0f);

    [Header("Transition")]
    public float transitionTime = 0.5f; // seconds

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minDistance = 3f;
    public float maxDistance = 10f;

    private Vector3 _targetPos;
    private Quaternion _targetRot;
    private float _transitionElapsed = 0f;
    private bool _isTransitioning = false;

    void Start()
    {
        transform.position = defaultPosition;
        transform.rotation = Quaternion.Euler(defaultRotation);
        _targetPos = defaultPosition;
        _targetRot = Quaternion.Euler(defaultRotation);
    }

    void LateUpdate()
    {
        HandleZoom();
        ApplyTransform();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            Vector3 dir = (transform.position - carTarget.position).normalized;
            float distance = Vector3.Distance(transform.position, carTarget.position);
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            transform.position = carTarget.position + dir * distance;
        }
    }

    void ApplyTransform()
    {
        if (_isTransitioning)
        {
            _transitionElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(_transitionElapsed / transitionTime);
            t = t * t * (3f - 2f * t); // SmoothStep easing

            transform.position = Vector3.Lerp(transform.position, _targetPos, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot, t);

            if (t >= 1f)
            {
                transform.position = _targetPos;
                transform.rotation = _targetRot;
                _isTransitioning = false;
            }
        }
    }

    // --- UI Calls ---

    public void SlideForColorPicker()
    {
        _targetPos = colorPosition;
        _targetRot = Quaternion.Euler(colorRotation);
        _transitionElapsed = 0f;
        _isTransitioning = true;
    }

    public void SlideToDefault()
    {
        _targetPos = defaultPosition;
        _targetRot = Quaternion.Euler(defaultRotation);
        _transitionElapsed = 0f;
        _isTransitioning = true;
    }
}