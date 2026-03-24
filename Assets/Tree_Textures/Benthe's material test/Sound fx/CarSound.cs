using UnityEngine;
using UnityEngine.InputSystem;

public class CarSound : MonoBehaviour
{
    public AudioSource audioSource;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.wKey.isPressed)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}