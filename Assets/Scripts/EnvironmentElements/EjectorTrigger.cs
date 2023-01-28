using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjectorTrigger : MonoBehaviour
{
    [SerializeField] private AnimationClip animationClip;
    
    private Animator _ejectorAnimator;

    private void Start()
    {
        _ejectorAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Move>();
        if (player != null)
        {
            _ejectorAnimator.Play(animationClip.name);
        }
    }
}
