using UnityEngine;

public class CustomizationMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject respraysPanel;
    public GameObject colorPickerPanel;

    public ColorPickerUI colorPickerUI;

    public void OpenResprays()
    {
        mainMenuPanel.SetActive(false);
        respraysPanel.SetActive(true);
        colorPickerPanel.SetActive(false);
    }

    public void OpenColorPicker()
    {
        mainMenuPanel.SetActive(false);
        respraysPanel.SetActive(false);
        colorPickerPanel.SetActive(true);
        colorPickerUI.SetTarget();
    }

    public void BackToMain()
    {
        mainMenuPanel.SetActive(true);
        respraysPanel.SetActive(false);
        colorPickerPanel.SetActive(false);
    }

    public void BackToResprays()
    {
        mainMenuPanel.SetActive(false);
        respraysPanel.SetActive(true);
        colorPickerPanel.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenuPanel.SetActive(true);
        respraysPanel.SetActive(false);
        colorPickerPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
