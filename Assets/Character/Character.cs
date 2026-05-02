using UnityEngine;


[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{

    [SerializeField] private Animator animator;


    // Shows the character initially
    public void FadeIn()
    {
        animator.SetTrigger("FadeIn");
    }

    // Hides the character
    public void HideCharacter()
    {
        animator.SetTrigger("Hide");
    }

    // Set hidden bool
    public void SetHidden(bool value)
    {
        animator.SetBool("Hidden", value);
    }

    // Character Starts Talking
    public void StartTalking()
    {
        animator.SetBool("Talking", true);
    }

    // Character Stops Talking
    public void StopTalking()
    {
        animator.SetBool("Talking", false);
    }

}


