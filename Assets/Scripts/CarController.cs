using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Visual")]
    public MeshRenderer carRenderer;
    public Light[] headlights;

    public GameObject[] wheels;
    public GameObject[] extraLights;

    [Header("Audio")]
    public AudioSource hornSource;
    public AudioClip[] hornClips;

    [Header("Colors")]
    public Color[] availableColors;

    public CarUpgradeData upgradeData;

    public void ApplyUpgrades()
    {
        // kleur
        if (upgradeData.colorIndex < availableColors.Length)
            carRenderer.material.color = availableColors[upgradeData.colorIndex];

        // koplampen
        foreach (Light light in headlights)
            light.enabled = upgradeData.headlightsOn;

        // banden
        for (int i = 0; i < wheels.Length; i++)
            wheels[i].SetActive(i == upgradeData.wheelIndex);

        // extra lamp
        for (int i = 0; i < extraLights.Length; i++)
            extraLights[i].SetActive(i == upgradeData.lightIndex);
    }

    public void PlayHorn()
    {
        if (upgradeData.hornIndex < hornClips.Length)
        {
            hornSource.clip = hornClips[upgradeData.hornIndex];
            hornSource.Play();
        }
    }
}