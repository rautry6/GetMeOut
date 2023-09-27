using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorButton : MonoBehaviour
{
    [SerializeField] private GameObject barrier;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private SpriteRenderer buttonSpriteRenderer;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            buttonSpriteRenderer.sprite = pressedSprite;
            barrier.SetActive(false);
        }
    }
}
