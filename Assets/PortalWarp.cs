using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PortalWarp : MonoBehaviour
{
    [SerializeField] private Animator portalAnimator;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform portalPosition;
    private static readonly int Warp = Animator.StringToHash("Warp");

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            portalAnimator.SetTrigger(Warp);
        }
    }
}