using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
    private static readonly int Empty = Animator.StringToHash("Empty");
    private static readonly int Finished = Animator.StringToHash("Finished");

    public void StartPowerUpRoutine()
    {
        playerMove.StopMovement();
        playerJump.DisableJumping();
        StartCoroutine(MovePlayerToPosition());

    }

    private IEnumerator MovePlayerToPosition()
    {
        var rigidBody = player.GetComponent<Rigidbody2D>();
        powerUpMachineSR.sortingOrder = 6;
        var seq = player.transform.DOMove(positionToMoveTo.position, movementTime).OnPlay(() =>
        {
            playerAnimations.ChangeAnimationState(AnimationState.PowerUp, "Player_PowerUp");
            powerUpParticles.Play();
        }).OnComplete(() =>
        {
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
        injector.SetActive(true);
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerMove.RegainMovement();
        playerJump.EnableJumping();
        powerUpMachineSR.sortingOrder = 1;
    }
}