using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    public Animator PlayerAnimator => playerAnimator;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private AnimationState _currentAnimationState = AnimationState.Idle;
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Jump = Animator.StringToHash("Jump");

    /*private void Start()
    {
        playerAnimator.Play("Player_Idle");
    }*/

    public void UpdateHorizontalValue(float speed)
    {
        if (speed != 0)
        {
            spriteRenderer.flipX = speed < 0;
        }
        
        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Jump"))
        {
            playerAnimator.SetFloat(Horizontal, Mathf.Abs(speed));
        }
    }

    /*public void ChangeAnimationState(AnimationState newState, string desiredAnimation)
    {
        if (_currentAnimationState == newState) return;

        _currentAnimationState = newState;

        if (newState == AnimationState.RunningLeft)
            spriteRenderer.flipX = true;
        else if (newState == AnimationState.RunningRight) spriteRenderer.flipX = false;

        playerAnimator.Play(desiredAnimation);
    }*/

    public void TriggerJump()
    {
        playerAnimator.SetTrigger(Jump);    
    }
}