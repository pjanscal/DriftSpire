using UnityEngine;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    public VehicleProfile vehicleProfile;
    public CarAppearance carAppearance;

    public Slider bodyRSlider;
    public Slider bodyGSlider;
    public Slider bodyBSlider;
    public Slider bodyMetallicSlider;
    public Slider bodyGlossSlider;

    public void SetTarget()
    {
        bodyRSlider.value = vehicleProfile.bodyR;
        bodyGSlider.value = vehicleProfile.bodyG;
        bodyBSlider.value = vehicleProfile.bodyB;
        bodyMetallicSlider.value = vehicleProfile.bodyMetallic;
        bodyGlossSlider.value = vehicleProfile.bodyGloss;
    }

    public void OnSliderChanged()
    {
        vehicleProfile.bodyR = bodyRSlider.value;
        vehicleProfile.bodyG = bodyGSlider.value;
        vehicleProfile.bodyB = bodyBSlider.value;
        vehicleProfile.bodyMetallic = bodyMetallicSlider.value;
        vehicleProfile.bodyGloss = bodyGlossSlider.value;

        carAppearance.ApplyBodyColor();
        vehicleProfile.Save();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
