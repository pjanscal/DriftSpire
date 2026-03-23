using UnityEngine;

public class CarAppearance : MonoBehaviour
{
    public VehicleProfile vehicleProfile;
    public MeshRenderer bodyRenderer;

    public void ApplyBodyColor()
    {
        bodyRenderer.material.color = new Color(vehicleProfile.bodyR, vehicleProfile.bodyG, vehicleProfile.bodyB);

        bodyRenderer.material.SetFloat("_Metallic", vehicleProfile.bodyMetallic);
        bodyRenderer.material.SetFloat("_Glossiness", vehicleProfile.bodyGloss);




        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            vehicleProfile.Load();
            ApplyBodyColor();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
