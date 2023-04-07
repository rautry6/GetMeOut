using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerUpRoutine : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Move playerMove;
    [SerializeField] private Jump playerJump;
    [SerializeField] private Transform positionToMoveTo;
    [SerializeField] private float powerUpDelay;
    [SerializeField] private float movementTime;
    [SerializeField] private SpriteRenderer powerUpMachineSR;
    [SerializeField] private Animator powerUpAnimator;
    [SerializeField] private GameObject injector;
    [SerializeField] private ParticleSystem powerUpParticles;
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private BoxCollider2D powerUpCollider;
    [SerializeField] private Light2D light;
    [SerializeField] private GameObject PowerUpUI;
    [SerializeField] private CinemachineBlendListCamera cinemachineBlendListCamera;
    [SerializeField] private AcidManager acidManager;
    [SerializeField] private Transform doorBlock;
    [SerializeField] private Transform doorEndPosition;

    private static readonly int Empty = Animator.StringToHash("Empty");
    private static readonly int Finished = Animator.StringToHash("Finished");
    [SerializeField] private float graphicActiveTime;
    private static readonly int Full = Animator.StringToHash("Full");

    public void StartWallPowerUpRoutine()
    {
        playerMove.StopMovement();
        playerJump.DisableJumping();
        PowerUpSequence();
    }

    public void StartJumpPowerUpRoutine()
    {
        playerMove.StopMovement();
        playerJump.DisableJumping();
        WallPowerUpSequence();
    }

    private void WallPowerUpSequence()
    {
        var rigidBody = player.GetComponent<Rigidbody2D>();
        powerUpMachineSR.sortingOrder = 6;
        var seq = player.transform.DOMove(positionToMoveTo.position, movementTime).OnPlay(() =>
        {
            powerUpCollider.enabled = false;
            injector.SetActive(true);
            playerAnimations.ChangeAnimationState(AnimationState.PowerUp, "Player_PowerUp");
        }).OnComplete(() =>
        {
            powerUpAnimator.SetTrigger(Full);
            powerUpParticles.Play();
            PowerUpUI.SetActive(true);
            player.transform.position = positionToMoveTo.position;
            if (rigidBody != null)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(JumpPlayerReachedSpot(rigidBody));
            }
        });
    }

    private void PowerUpSequence()
    {
        var rigidBody = player.GetComponent<Rigidbody2D>();
        powerUpMachineSR.sortingOrder = 6;
        var seq = player.transform.DOMove(positionToMoveTo.position, movementTime).OnPlay(() =>
        {
            powerUpCollider.enabled = false;
            injector.SetActive(true);
            playerAnimations.ChangeAnimationState(AnimationState.PowerUp, "Player_PowerUp");
        }).OnComplete(() =>
        {
            powerUpAnimator.SetTrigger(Full);
            powerUpParticles.Play();
            PowerUpUI.SetActive(true);
            player.transform.position = positionToMoveTo.position;
            if (rigidBody != null)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(PlayerReachedSpot(rigidBody));
            }
        });
    }

    private IEnumerator PlayerReachedSpot(Rigidbody2D rigidBody)
    {
        yield return new WaitForSeconds(3f);
        powerUpParticles.Stop();
        light.intensity = 0;
        yield return new WaitForSeconds(2.33f);
        cinemachineBlendListCamera.enabled = true;
        cinemachineBlendListCamera.Priority = 100;
        yield return new WaitForSeconds(4f);
        acidManager.HandleStartAcid();
        yield return new WaitForSeconds(2f);
        cinemachineBlendListCamera.Priority = 0;
        powerUpMachineSR.sortingOrder = 1;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerMove.RegainMovement();
        playerJump.EnableJumping();
        yield return new WaitForSeconds(graphicActiveTime);
        PowerUpUI.SetActive(false);
    }

    private IEnumerator JumpPlayerReachedSpot(Rigidbody2D rigidBody)
    {
        yield return new WaitForSeconds(2f);
        powerUpParticles.Stop();
        light.intensity = 0;
        powerUpMachineSR.sortingOrder = 1;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        doorBlock.DOMoveY(doorEndPosition.position.y, 2f);
        playerMove.RegainMovement();
        playerJump.EnableJumping();
        yield return new WaitForSeconds(graphicActiveTime);
        PowerUpUI.SetActive(false);
    }
}