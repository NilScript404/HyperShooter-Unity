using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Animator animator;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Play(string animationName)
    {
        animator.Play(animationName);
    }

    public void Play(string animationName, float animatorSpeed)
    {
        animator.speed = animatorSpeed;
        Play(animationName);
    }

    public void PlayIdleAnimation()
    {
        Play("Idle");
    }

    public void PlayRunAnimation()
    {
        Play("Run");
    }

}
