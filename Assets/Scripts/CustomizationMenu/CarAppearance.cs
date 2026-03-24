using JetBrains.Annotations;
using UnityEngine;

public class CarAppearance : MonoBehaviour
{
    public VehicleProfile vehicleProfile;
    public MeshRenderer bodyRenderer;
    public MeshRenderer[] wheelRenderers;
    public MeshRenderer[] headlightRenderers;

    public void ApplyBodyColor()
    {
        bodyRenderer.material.color = new Color(vehicleProfile.bodyR, vehicleProfile.bodyG, vehicleProfile.bodyB);

        bodyRenderer.material.SetFloat("_Metallic", vehicleProfile.bodyMetallic);
        bodyRenderer.material.SetFloat("_Smoothness", vehicleProfile.bodyGloss);

        foreach (MeshRenderer headlight in headlightRenderers)
        {
            headlight.material.color = new Color(vehicleProfile.bodyR, vehicleProfile.bodyG, vehicleProfile.bodyB);
            headlight.material.SetFloat("_Metallic", vehicleProfile.bodyMetallic);
            headlight.material.SetFloat("_Smoothness", vehicleProfile.bodyGloss);
        }

    }




    public void ApplyWheelColor()
    {
        foreach (MeshRenderer wheel in wheelRenderers)
        {
            wheel.material.color = new Color(vehicleProfile.wheelR, vehicleProfile.wheelG, vehicleProfile.wheelB);
            wheel.material.SetFloat("_Metallic", vehicleProfile.wheelMetallic);
            wheel.material.SetFloat("_Smoothness", vehicleProfile.wheelGloss);
        }
    }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            vehicleProfile.Load();
            ApplyBodyColor();
            ApplyWheelColor();
        }

        // Update is called once per frame
        void Update()
        {

        }
}

