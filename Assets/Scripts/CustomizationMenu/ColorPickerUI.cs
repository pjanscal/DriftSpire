using UnityEngine;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    public VehicleProfile vehicleProfile;
    public CarAppearance carAppearance;

    [Header("Body Sliders")]
    public Slider bodyRSlider;
    public Slider bodyGSlider;
    public Slider bodyBSlider;
    public Slider bodyGlossSlider; // only gloss now

    public enum CarPart { Body }
    private CarPart currentPart;

    public void SetTarget(CarPart part)
    {
        currentPart = part;

        // Populate sliders with saved values
        bodyRSlider.value = vehicleProfile.bodyR;
        bodyGSlider.value = vehicleProfile.bodyG;
        bodyBSlider.value = vehicleProfile.bodyB;
        bodyGlossSlider.value = vehicleProfile.bodyGloss;
    }

    public void OnSliderChanged()
    {
        vehicleProfile.bodyR = bodyRSlider.value;
        vehicleProfile.bodyG = bodyGSlider.value;
        vehicleProfile.bodyB = bodyBSlider.value;
        vehicleProfile.bodyGloss = bodyGlossSlider.value;

        // Apply appearance immediately
        carAppearance.ApplyBodyAppearance();

        // Save changes
        vehicleProfile.Save();
    }
}