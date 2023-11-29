using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardHolder : MonoBehaviour
{
    [SerializeField] private KeyCard keyCard;
    [SerializeField] private KeyCardManager keyCardManager;
    [SerializeField] private KeyCardUI keyCardUI;
    
    private SpriteRenderer _spriteRenderer;
    private Action _keyCardCollected;
        

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = keyCard.KeyCardSprite;
        _spriteRenderer.color = keyCard.CardColorValue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            keyCardManager.OnKeyCardCollected(keyCard, gameObject);
       
            gameObject.SetActive(false);
        }
    }

    public void AddKeyCardUI()
    {
        keyCardUI.AddKeyCardUI(keyCard);    
    }
}
