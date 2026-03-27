using UnityEngine;

public class VehicleProfile : MonoBehaviour
{
    public float bodyR = 1f;
    public float bodyG = 1f;
    public float bodyB = 1f;
    public float bodyGloss = 0.5f; // only smoothness now

    public void Save()
    {
        PlayerPrefs.SetFloat("bodyR", bodyR);
        PlayerPrefs.SetFloat("bodyG", bodyG);
        PlayerPrefs.SetFloat("bodyB", bodyB);
        PlayerPrefs.SetFloat("bodyGloss", bodyGloss);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        bodyR = PlayerPrefs.GetFloat("bodyR", 1f);
        bodyG = PlayerPrefs.GetFloat("bodyG", 1f);
        bodyB = PlayerPrefs.GetFloat("bodyB", 1f);
        bodyGloss = PlayerPrefs.GetFloat("bodyGloss", 0.5f);
    }
}