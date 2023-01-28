using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class PortalWarp : MonoBehaviour
{
    [SerializeField] private Animator portalAnimator;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform portalPosition;
    [SerializeField] private float portalWarpDelay = 1f;
    [SerializeField] private Animator doorAnimator;
    private static readonly int Warp = Animator.StringToHash("Warp");
    private static readonly int Open = Animator.StringToHash("Open");

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            MovePlayerToMiddlePortal(other);
        }
    }

    private void MovePlayerToMiddlePortal(Component other)
    {
        other.gameObject.transform.DOMove(portalPosition.position, portalWarpDelay).OnComplete(() =>
        {
            portalAnimator.SetTrigger(Warp);
            doorAnimator.SetTrigger(Open);
        });
    }
}