using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float respawnTime;
    [SerializeField] private bool shouldFall;
    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            if(shouldFall)
                StartCoroutine(BeginFalling());
    }

    private IEnumerator BeginFalling()
    {
        yield return new WaitForSeconds(delay);
        _boxCollider2D.enabled = false;
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        _boxCollider2D.enabled = true;
        _spriteRenderer.enabled = true;
    }
}