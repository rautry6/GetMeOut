using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class SaveAnimatorController : MonoBehaviour
{
    [SerializeField] private AnimationClip saveIdle;
    [SerializeField] private AnimationClip saveDown;
    [SerializeField] private AnimationClip saveStart;
    [SerializeField] private KeyCardColors doorColor;
    [SerializeField] private Animator doorAnimator;
    
    private static readonly int Open = Animator.StringToHash("Open");

    private Animator _saveAnimator;

    private void Start()
    {
        _saveAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Move>();
        if (player != null)
        {
            var keyCardManager = player.GetComponentInChildren<KeyCardManager>();
            if (keyCardManager.CheckIfManagerHasCorrectCard(doorColor))
            {
                _saveAnimator.Play(saveStart.name);
            }
        }
    }

    // Animation event 
    private void PlayIdleAnimation()
    {
        _saveAnimator.CrossFade(saveIdle.name, .5f);
        doorAnimator.SetTrigger(Open);
    }
    
}
