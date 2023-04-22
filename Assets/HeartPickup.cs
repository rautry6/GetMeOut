using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private Vector3 punchScale;
    [SerializeField] private float duration;
    [SerializeField] private int vibrato;
    [SerializeField] private float elasticity;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {

        transform.DOPunchScale(punchScale, duration, vibrato, elasticity).SetLoops(-1);
    }
}
