using UnityEngine;

public class MenuAnimationController : MonoBehaviour
{
    public Animator animator;
    public string animName = "rig|CarLeanIdle";

    public static bool hasPlayed = false;


    void Start()
    {
         
        if (hasPlayed)
        {
            // Skip naar einde van animatie
            animator.Play(animName, 0, 0.99f);
        }
        else
        {
            hasPlayed = true;
        }
    }
    
}