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
    [SerializeField] private PowerUpTypes power;

    private static readonly int Empty = Animator.StringToHash("Empty");
    private static readonly int Finished = Animator.StringToHash("Finished");
    [SerializeField] private float graphicActiveTime;
    private static readonly int Full = Animator.StringToHash("Full");

    public void PowerUpRoutineStarted()
    {
        playerMove.StopMovement();
        playerJump.DisableJumping();

        switch (power)
        {

            case PowerUpTypes.WallInteractor:
                {
                    StartWallPowerUpRoutine();
                    break;
                }
            case PowerUpTypes.DoubleJump:
                {
                    StartPowerUpRoutine("DoubleJump"); 
                    break;
                }
            case PowerUpTypes.Grapple:
                {
                    StartPowerUpRoutine("Grapple");
                    break;
                }
            case PowerUpTypes.Dash:
                {
                    StartPowerUpRoutine("Dash");
                    break;
                }
        }
    }

    public void StartWallPowerUpRoutine()
    {
        WallPowerUpSequence();
    }

    public void StartPowerUpRoutine(string powerUpName)
    {
        var rigidBody = player.GetComponent<Rigidbody2D>();
        powerUpMachineSR.sortingOrder = 6;
        var seq = player.transform.DOMove(positionToMoveTo.position, movementTime).OnPlay(() =>
        {
            powerUpCollider.enabled = false;
            injector.SetActive(true);
            playerAnimations.PlayerAnimator.Play("Player_PowerUp");
        }).OnComplete(() =>
        {
            powerUpAnimator.SetTrigger(Full);
            powerUpParticles.Play();
            PowerUpUI.SetActive(true);
            PowerUpManager.Instance.PowerUpList.Add(powerUpName);
            player.transform.position = positionToMoveTo.position;
            if (rigidBody != null)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(PlayerReachedSpot(rigidBody));
            }
        });
    }

    private void WallPowerUpSequence()
    {
        var rigidBody = player.GetComponent<Rigidbody2D>();
        powerUpMachineSR.sortingOrder = 6;
        var seq = player.transform.DOMove(positionToMoveTo.position, movementTime).OnPlay(() =>
        {
            powerUpCollider.enabled = false;
            injector.SetActive(true);
            //playerAnimations.ChangeAnimationState(AnimationState.PowerUp, "Player_PowerUp");
        }).OnComplete(() =>
        {
            powerUpAnimator.SetTrigger(Full);
            powerUpParticles.Play();
            PowerUpUI.SetActive(true);
            PowerUpManager.Instance.PowerUpList.Add("WallInteractor");
            player.transform.position = positionToMoveTo.position;
            if (rigidBody != null)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(WallPowerUpPlayerReachedSpot(rigidBody));
            }
        });
    }

    private IEnumerator WallPowerUpPlayerReachedSpot(Rigidbody2D rigidBody)
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

    private IEnumerator PlayerReachedSpot(Rigidbody2D rigidBody)
    {
        yield return new WaitForSeconds(2f);
        powerUpParticles.Stop();
        light.intensity = 0;
        powerUpMachineSR.sortingOrder = 1;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

        //Used for boss door lowering
        if (doorBlock != null)
        {
            doorBlock.DOMoveY(doorEndPosition.position.y, 2f);
            playerAnimations.PlayerAnimator.Play("Player_Idle");
        }

        playerMove.RegainMovement();
        playerJump.EnableJumping();

        if (power == PowerUpTypes.Grapple)
        {
            player.GetComponent<Grapple>().enabled = true;    
        }

        if (power == PowerUpTypes.Dash)
        {
            player.GetComponent<Dash>().enabled = true;
        }

        yield return new WaitForSeconds(graphicActiveTime);
        PowerUpUI.SetActive(false);
    }
}