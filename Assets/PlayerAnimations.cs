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

    private void Start()
    {
        playerAnimator.Play("Player_Idle");
    }

    public void ChangeAnimationState(AnimationState newState, string desiredAnimation)
    {
        Debug.Log(newState);
        if (_currentAnimationState == newState) return;
        
        _currentAnimationState = newState;

        if (newState == AnimationState.RunningLeft)
        {
            spriteRenderer.flipX = true;
        }
        else if(newState == AnimationState.RunningRight)
        {
            spriteRenderer.flipX = false;
        }
        
        playerAnimator.Play(desiredAnimation);

    }
}
