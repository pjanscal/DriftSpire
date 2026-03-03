using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Visual References")]
    public MeshRenderer carRenderer;
    public Light[] headlights;

    [Header("Audio")]
    public AudioSource hornSource;
    public AudioClip[] hornClips;

    [Header("Available Colors")]
    public Color[] availableColors;

    public CarUpgradeData upgradeData;

    public void ApplyUpgrades()
    {
        // Pas de kleur van de auto aan op basis van de gekozen index
        if (upgradeData.colorIndex >= 0 && upgradeData.colorIndex < availableColors.Length)
        {
            carRenderer.material.color = availableColors[upgradeData.colorIndex];
        }

        // Zet de koplampen aan of uit
        foreach (Light light in headlights)
        {
            light.enabled = upgradeData.headlightsOn;
        }
    }

    public void PlayHorn()
    {
        // Speel het juiste toetergeluid af
        if (upgradeData.hornIndex >= 0 && upgradeData.hornIndex < hornClips.Length)
        {
            hornSource.clip = hornClips[upgradeData.hornIndex];
            hornSource.Play();
        }
    }
}
