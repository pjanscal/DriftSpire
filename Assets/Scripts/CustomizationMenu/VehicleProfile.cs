using UnityEngine;

public class VehicleProfile : MonoBehaviour
{
    public float bodyR = 1f;
    public float bodyG = 1f;
    public float bodyB = 1f;
    public float bodyMetallic = 0f;
    public float bodyGloss = 0.5f;

    public float wheelR = 1f;
    public float wheelG = 1f;
    public float wheelB = 1f;
    public float wheelMetallic = 1f;
    public float wheelGloss = 1f;

    public void Save()
    {
        PlayerPrefs.SetFloat("bodyR", bodyR);
        PlayerPrefs.SetFloat("bodyG", bodyG);
        PlayerPrefs.SetFloat("bodyB", bodyB);
        PlayerPrefs.SetFloat("bodyMetallic", bodyMetallic);
        PlayerPrefs.SetFloat("bodyGloss", bodyGloss);

        PlayerPrefs.SetFloat("wheelR", wheelR);
        PlayerPrefs.SetFloat("wheelG", wheelG);
        PlayerPrefs.SetFloat("wheelB", wheelB);
        PlayerPrefs.SetFloat("wheelMetallic", wheelMetallic);
        PlayerPrefs.SetFloat("wheelGloss", wheelGloss);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        bodyR = PlayerPrefs.GetFloat("bodyR", 1f);
        bodyG = PlayerPrefs.GetFloat("bodyG", 1f);
        bodyB = PlayerPrefs.GetFloat("bodyB", 1f);
        bodyMetallic = PlayerPrefs.GetFloat("bodyMetallic", 0f);
        bodyGloss = PlayerPrefs.GetFloat("bodyGloss", 0.5f);

        wheelR = PlayerPrefs.GetFloat("wheelR", 1f);
        wheelG = PlayerPrefs.GetFloat("wheelG", 1f);
        wheelB = PlayerPrefs.GetFloat("wheelB", 1f);
        wheelMetallic = PlayerPrefs.GetFloat("wheelMetallic", 1f);
        wheelGloss = PlayerPrefs.GetFloat("wheelGloss", 1f);

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
