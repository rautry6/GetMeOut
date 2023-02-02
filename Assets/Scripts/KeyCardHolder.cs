using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardHolder : MonoBehaviour
{
    [SerializeField] private KeyCard keyCard;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = keyCard.KeyCardSprite;
        _spriteRenderer.color = keyCard.CardColorValue;
    }
}