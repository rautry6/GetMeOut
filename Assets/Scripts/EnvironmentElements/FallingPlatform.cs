using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float respawnTime;
    [SerializeField] private bool shouldFall;
    [SerializeField] float timeForSideCheck;

    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    private bool _detectedPlayer;
    private float _timeSincePlayerFound;
    private Animator _animator;
    private static readonly int PlayerLanded = Animator.StringToHash("PlayerLanded");
    [SerializeField] private bool isSlow;
    [SerializeField] private float animatorSpeed;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        foreach (var param in _animator.parameters)
        {
            Debug.Log(param);
        }
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

    private void DisableCollider()
    {
        _boxCollider2D.enabled = false;
        ;
    }

    private void EnableCollider()
    {
        if (_boxCollider2D.enabled) return;
        
        _boxCollider2D.enabled = true;
    }
}