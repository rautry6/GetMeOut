using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private bool shouldFall;
    [SerializeField] float timeForSideCheck;

    private bool _detectedPlayer;
    private float _timeSincePlayerFound;
    private Animator _animator;
    [SerializeField] private bool isSlow;
    [SerializeField] private float animatorSpeed;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (shouldFall)
            {
                _timeSincePlayerFound += Time.deltaTime;
                if (_timeSincePlayerFound > timeForSideCheck)
                {
                    _timeSincePlayerFound = 0f;
                    if (isSlow)
                    {
                        animatorSpeed = .66f;
                        _animator.speed = animatorSpeed;
                    }
                    else _animator.speed = 1f;
                    _animator.Play("FragilePlatformBegin");
                }
            }
        }
    }

    public void RotateEffector()
    {
        var effector = gameObject.GetComponent<PlatformEffector2D>();
        effector.rotationalOffset = 180f;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;


    }
    
    public void ResetEffector()
    {
        var effector = gameObject.GetComponent<PlatformEffector2D>();
        effector.rotationalOffset = 0f;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}