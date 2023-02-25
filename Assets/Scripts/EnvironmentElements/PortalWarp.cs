using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class PortalWarp : MonoBehaviour
{
    [SerializeField] private Animator portalAnimator;
    [SerializeField] private Transform portalPosition;
    [SerializeField] private float portalWarpDelay = 1f;
    [SerializeField] private float moveDuration;
    [SerializeField] private Transform finishPortalTransform;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private CapsuleCollider2D playerCollider;
    private static readonly int Warp = Animator.StringToHash("Warp");

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Move>() != null)
        {
            MovePlayerToMiddlePortal(other);
        }
    }

    private void MovePlayerToMiddlePortal(Component other)
    {
        other.gameObject.transform.DOMove(portalPosition.position, portalWarpDelay).OnComplete(() =>
        {
            portalAnimator.SetTrigger(Warp);
            playerSpriteRenderer.enabled = false;
            var finishPosition = finishPortalTransform.position;
            other.gameObject.transform.DOMove(finishPosition, moveDuration).OnComplete(() =>
            {
                playerSpriteRenderer.enabled = true;
            });
        });
    }
}