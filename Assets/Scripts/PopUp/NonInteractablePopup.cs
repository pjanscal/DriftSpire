using UnityEngine;
using UnityEngine.UI; // Voor Button
using UnityEngine.EventSystems; // Voor IPointerClickHandler

public class NonInteractablePopup : MonoBehaviour, IPointerClickHandler
{
    public Canvas popupCanvas; // de canvas die je wilt laten zien
    private Button button; // de button die we checken

    void Start()
    {
        button = GetComponent<Button>();
        if(popupCanvas != null)
            popupCanvas.gameObject.SetActive(false); // popup uitzetten bij start
    }

    // Wordt aangeroepen als er op het UI element wordt geklikt
    public void OnPointerClick(PointerEventData eventData)
    {
        if (button != null && !button.interactable) 
        {
            // Button is NIET interactable → toon de popup
            if(popupCanvas != null)
                popupCanvas.gameObject.SetActive(true);
        }
    }

    // Optioneel: functie om de popup weer te sluiten
    public void ClosePopup()
    {
        if(popupCanvas != null)
            popupCanvas.gameObject.SetActive(false);
    }
}