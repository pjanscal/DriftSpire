using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CustomizationMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject colorPickerPanel;
    public GameObject upgradesPanel;

    [Header("Components")]
    public ColorPickerUI colorPickerUI;
    public CustomizationCamera customizationCamera;

    public UpgradeManager upgradeManager;

    private bool isInColorPicker = false;

    void Start()
    {
        // Start in Main Menu
        mainMenuPanel.SetActive(true);
        colorPickerPanel.SetActive(false);
        upgradesPanel.SetActive(false);
    }

    // --- MAIN MENU ---
    public void OpenRespray()
    {
        mainMenuPanel.SetActive(false);
        colorPickerPanel.SetActive(true);

        isInColorPicker = true;
        colorPickerUI.SetTarget(ColorPickerUI.CarPart.Body);

        // Force camera update next frame
        StopAllCoroutines();
        StartCoroutine(ActivateCameraNextFrame());
    }

    public void BackToMain()
    {
        mainMenuPanel.SetActive(true);
        colorPickerPanel.SetActive(false);

        if (isInColorPicker)
        {
            customizationCamera.SlideToDefault();
            isInColorPicker = false;
        }
    }

    public void OpenUpgrades()
    {
        mainMenuPanel.SetActive(false);
        upgradesPanel.SetActive(true);
    }

    public void BackToMainFromUpgrades()
    {
        mainMenuPanel.SetActive(true);
        upgradesPanel.SetActive(false);
    }
    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene("StartUiScene");
    }

    IEnumerator ActivateCameraNextFrame()
    {
        yield return null; // wait 1 frame for UI to settle
        customizationCamera.SlideForColorPicker();
    }
}