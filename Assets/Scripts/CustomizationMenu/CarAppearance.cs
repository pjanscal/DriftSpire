using UnityEngine;

public class CarAppearance : MonoBehaviour
{
    public VehicleProfile vehicleProfile;
    public MeshRenderer bodyRenderer;
    public MeshRenderer[] headlightRenderers;

    void Start()
    {
        vehicleProfile.Load();
        ApplyBodyAppearance();
    }

    public void ApplyBodyAppearance()
    {
        // Ensure we use a unique material instance
        Material mat = bodyRenderer.material;

        // Set base color
        Color bodyColor = new Color(vehicleProfile.bodyR, vehicleProfile.bodyG, vehicleProfile.bodyB);
        mat.color = bodyColor;

        // Set smoothness (gloss)
        mat.SetFloat("_Smoothness", vehicleProfile.bodyGloss);

        // Apply same appearance to headlights
        foreach (MeshRenderer headlight in headlightRenderers)
        {
            Material headMat = headlight.material;
            headMat.color = bodyColor;
            headMat.SetFloat("_Smoothness", vehicleProfile.bodyGloss);
        }
    }
}