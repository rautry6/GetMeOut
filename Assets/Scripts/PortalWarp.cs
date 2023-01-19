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
    private static readonly int Warp = Animator.StringToHash("Warp");

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            MovePlayerToMiddlePortal(other);
        }
    }

    private void MovePlayerToMiddlePortal(Component other)
    {
        var playerRun = other.GetComponent<PlayerRun>();
        playerRun.CanMove = false;
        other.gameObject.transform.DOMove(portalPosition.position, portalWarpDelay).OnComplete(() =>
        {
            portalAnimator.SetTrigger(Warp);
            playerRun.CanMove = true;
        });
    }
}