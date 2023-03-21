using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private static readonly int Empty = Animator.StringToHash("Empty");
    private static readonly int Finished = Animator.StringToHash("Finished");
    [SerializeField] private float graphicActiveTime;

    public void StartPowerUpRoutine()
    {
        playerMove.StopMovement();
        playerJump.DisableJumping();
        StartCoroutine(PowerUpSequence());

    }

    private IEnumerator PowerUpSequence()
    {
        var rigidBody = player.GetComponent<Rigidbody2D>();
        powerUpMachineSR.sortingOrder = 6;
        var seq = player.transform.DOMove(positionToMoveTo.position, movementTime).OnPlay(() =>
        {
            powerUpCollider.enabled = false;
            injector.SetActive(true);
            playerAnimations.ChangeAnimationState(AnimationState.PowerUp, "Player_PowerUp");
            powerUpParticles.Play();
        }).OnComplete(() =>
        {
            PowerUpUI.SetActive(true);
            player.transform.position = positionToMoveTo.position;
            if (rigidBody != null)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            powerUpParticles.Stop();

        });
        yield return new WaitUntil(() => seq.active == false);
        powerUpAnimator.SetTrigger(Empty);
        yield return new WaitForSeconds(powerUpAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length * powerUpDelay);
        powerUpAnimator.SetTrigger(Finished);
        light.intensity = 0;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerMove.RegainMovement();
        playerJump.EnableJumping();
        powerUpMachineSR.sortingOrder = 1;
        yield return new WaitForSeconds(graphicActiveTime);
        PowerUpUI.SetActive(false);
    }
}