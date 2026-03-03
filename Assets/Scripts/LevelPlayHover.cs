using UnityEngine;
using UnityEngine.EventSystems;



public class LevelPlayHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MapHoverZoom mapZoom;
    private Vector3 originalScale; 
    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.1f; 
        mapZoom.FocusOn((RectTransform)transform);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        mapZoom.ResetView();
    }
    

}
