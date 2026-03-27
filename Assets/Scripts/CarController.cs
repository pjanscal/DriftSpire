//using UnityEngine;

//public class CarController : MonoBehaviour
//{
//    [Header("Visual")]
//    public MeshRenderer carRenderer; // Het materiaal van de auto (voor kleur aanpassen)
//    public Light[] headlights; // Alle koplampen van de auto

//    public GameObject[] wheels; // Verschillende wiel modellen
//    public GameObject[] extraLights; // Extra lampen op de auto

//    [Header("Audio")]
//    public AudioSource hornSource; // AudioSource voor het afspelen van de claxon
//    public AudioClip[] hornClips; // Verschillende horn geluiden

//    [Header("Colors")]
//    public Color[] availableColors; // Alle mogelijke kleuren

//    public CarUpgradeData upgradeData; // Hierin wordt alles opgeslagen wat de speler gekozen heeft

//    public void ApplyUpgrades()
//    {
//        // 🎨 KLEUR
//        // Checkt of de gekozen kleur bestaat
//        if (upgradeData.colorIndex < availableColors.Length)
//            carRenderer.material.color = availableColors[upgradeData.colorIndex];

//        // 💡 KOPLAMPEN
//        // Zet alle koplampen aan/uit afhankelijk van de save data
//        foreach (Light light in headlights)
//            light.enabled = upgradeData.headlightsOn;

//        // 🛞 BANDEN
//        // Zorgt dat maar 1 wiel set actief is
//        for (int i = 0; i < wheels.Length; i++)
//            wheels[i].SetActive(i == upgradeData.wheelIndex);

//        // 🔦 EXTRA LAMPEN
//        // Zelfde principe als wielen: maar 1 actief
//        for (int i = 0; i < extraLights.Length; i++)
//            extraLights[i].SetActive(i == upgradeData.lightIndex);
//    }

//    public void PlayHorn()
//    {
//        // 📢 CLAXON
//        // Checkt of de gekozen horn bestaat
//        if (upgradeData.hornIndex < hornClips.Length)
//        {
//            // Zet de juiste audio clip
//            hornSource.clip = hornClips[upgradeData.hornIndex];

//            // Speel het geluid af
//            hornSource.Play();
//        }
//    }
//}