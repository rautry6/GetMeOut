using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainAcidButtonPress : MonoBehaviour
{
    [SerializeField] private SpriteRenderer buttonSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private AcidManager acidManager;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            buttonSprite.sprite = pressedSprite;
            acidManager.DrainAcid();
        }
    }
}