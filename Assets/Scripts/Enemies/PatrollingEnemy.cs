using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    [SerializeField] private Ease easeType;
    [SerializeField] private Transform leftWaypoint;
    [SerializeField] private Transform rightWaypoint;
    [SerializeField] private float moveDuration;
    [SerializeField] private float idleTime;
    [SerializeField] private MoveDirection _currentMoveDirection;
    [SerializeField] private Animator animator;

    private enum MoveDirection
    {
        Right,
        Left
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (_currentMoveDirection == MoveDirection.Left)
        {
            MoveLeft();
        }
        else
        {
            MoveRight();
        }
    }

    public void MoveRight()
    {
        if (transform != null)
        {
            transform.DOMoveX(rightWaypoint.position.x, moveDuration).SetEase(easeType).OnPlay(() =>
            {
                animator.SetTrigger("WalkRight");
            }).OnComplete(() =>
            {
                animator.SetTrigger("TurnLeft");
                StartCoroutine(Idle());
            });
        }
    }

    public void MoveLeft()
    {
        if (transform != null)
        {
            transform.DOMoveX(leftWaypoint.position.x, moveDuration).SetEase(easeType).OnPlay(() =>
            {
                animator.SetTrigger("WalkLeft");
            }).OnComplete(() =>
            {
                animator.SetTrigger("TurnRight");
                StartCoroutine(Idle());
            });
        }
    }

    public IEnumerator Idle()
    {
        yield return new WaitForSeconds(idleTime);

        if (_currentMoveDirection == MoveDirection.Left)
        {
            _currentMoveDirection = MoveDirection.Right;
            MoveRight();
        }
        else
        {
            _currentMoveDirection = MoveDirection.Left;
            MoveLeft();
        }
    }

}