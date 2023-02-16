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
    [SerializeField] private Rigidbody2D playerRigidbody;
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
            var finishPosition = finishPortalTransform.position;
            playerSpriteRenderer.enabled = false;
            playerRigidbody.isKinematic = true;
            Debug.Log(finishPosition);
            other.gameObject.transform.DOMoveY(finishPosition.y, moveDuration).OnComplete(() =>
            {
                other.gameObject.transform.DOMoveX(finishPosition.x, moveDuration).OnComplete(() =>
                {
                    playerRigidbody.isKinematic = false;
                    playerSpriteRenderer.enabled = true;
                });

            });
        });
    }
}