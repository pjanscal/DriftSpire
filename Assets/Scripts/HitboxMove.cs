using UnityEngine;
using UnityEngine.EventSystems;



public class HitboxMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MapHoverZoom mapZoom;
    public void OnPointerEnter(PointerEventData eventData)
    {
        mapZoom.FocusOn((RectTransform)transform);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mapZoom.ResetView();
    }
    

}
