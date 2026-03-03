using UnityEngine;
using UnityEngine.EventSystems;

public class MapHoverZoom : MonoBehaviour
{
    public RectTransform mapContainer;
    public float zoomScale = 1.5f;
    public float speed = 3f;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    private Vector3 targetScale;
    private Vector3 targetPosition;

    void Start()
    {
        originalScale = mapContainer.localScale;
        originalPosition = mapContainer.localPosition;

        targetScale = originalScale;
        targetPosition = originalPosition;
    }

    void Update()
    {
        mapContainer.localScale = Vector3.Lerp(mapContainer.localScale, targetScale, Time.deltaTime * speed);
        mapContainer.localPosition = Vector3.Lerp(mapContainer.localPosition, targetPosition, Time.deltaTime * speed);
    }

    public void FocusOn(RectTransform level)
    {
        targetScale = Vector3.one * zoomScale;
        targetPosition = -level.localPosition * zoomScale;
    }

    public void ResetView()
    {
        targetScale = originalScale;
        targetPosition = originalPosition;
    }
}
