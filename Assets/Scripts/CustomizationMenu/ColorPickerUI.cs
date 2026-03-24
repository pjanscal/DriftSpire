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

    public Slider wheelRSlider;
    public Slider wheelBSlider;
    public Slider wheelGSlider;
    public Slider wheelMetallicSlider;
    public Slider wheelGlossSlider;

    public enum CarPart { Body, Wheels }
    private CarPart currentPart;

    public void SetTarget(CarPart part)
    {
        currentPart = part;

        if (part == CarPart.Body)
        {

            bodyRSlider.value = vehicleProfile.bodyR;
            bodyGSlider.value = vehicleProfile.bodyG;
            bodyBSlider.value = vehicleProfile.bodyB;
            bodyMetallicSlider.value = vehicleProfile.bodyMetallic;
            bodyGlossSlider.value = vehicleProfile.bodyGloss;
        }
        else
        {

            bodyRSlider.value = vehicleProfile.wheelR;
            bodyGSlider.value = vehicleProfile.wheelG;
            bodyBSlider.value = vehicleProfile.wheelB;
            bodyMetallicSlider.value = vehicleProfile.wheelMetallic;
            bodyGlossSlider.value = vehicleProfile.wheelGloss;
        }
    }

    public void OnSliderChanged()
    {
        if (currentPart == CarPart.Body)
        {
        vehicleProfile.bodyR = bodyRSlider.value;
        vehicleProfile.bodyG = bodyGSlider.value;
        vehicleProfile.bodyB = bodyBSlider.value;
        vehicleProfile.bodyMetallic = bodyMetallicSlider.value;
        vehicleProfile.bodyGloss = bodyGlossSlider.value;
        carAppearance.ApplyBodyColor();
        }
        else
        {
        vehicleProfile.wheelR = bodyRSlider.value;
        vehicleProfile.wheelG = bodyGSlider.value;
        vehicleProfile.wheelB = bodyBSlider.value;
        vehicleProfile.wheelMetallic = bodyMetallicSlider.value;
        vehicleProfile.wheelGloss = bodyGlossSlider.value;
        carAppearance.ApplyWheelColor();
        }

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
