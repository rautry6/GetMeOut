using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CapsuleCollider2D hitBox;

    private CurrentAnimation currentAnimation;
    private Transform endPosition;
    private bool attacking = false;
    private enum CurrentAnimation
    {
        WalkRight,
        WalkLeft,
        TurnRight,
        TurnLeft,
    }

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
            endPosition = leftWaypoint;
            MoveLeft();
        }
        else
        {
            endPosition = rightWaypoint;
            MoveRight();
        }
    }

    public void MoveRight()
    {
        spriteRenderer.flipX = true;
        if (transform != null)
        {
            transform.DOMoveX(rightWaypoint.position.x, CalculateMoveTime()).SetEase(easeType).OnPlay(() =>
            {
                animator.SetTrigger("WalkLeft");
                currentAnimation = CurrentAnimation.WalkRight;
            }).OnComplete(() =>
            {
                spriteRenderer.flipX = false;
                animator.SetTrigger("TurnLeft");
                currentAnimation = CurrentAnimation.TurnLeft;
                StartCoroutine(Idle());
            });
        }
    }

    public void MoveLeft()
    {
        spriteRenderer.flipX = false;
        if (transform != null)
        {
            transform.DOMoveX(leftWaypoint.position.x, CalculateMoveTime()).SetEase(easeType).OnPlay(() =>
            {
                animator.SetTrigger("WalkLeft");
                currentAnimation = CurrentAnimation.WalkLeft;
            }).OnComplete(() =>
            {
                animator.SetTrigger("TurnLeft");
                currentAnimation = CurrentAnimation.TurnRight;
                spriteRenderer.flipX = true;
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
            endPosition = rightWaypoint;
            MoveRight();
        }
        else
        {
            _currentMoveDirection = MoveDirection.Left;
            endPosition = leftWaypoint;
            MoveLeft();
        }
    }

    public void Attack(GameObject player)
    {
        if (!attacking && CheckIfCanAttack(player.transform))
        {
            attacking = true;

            animator.SetTrigger("Attack");
           
            StartCoroutine(Resume());
        }
    }

    IEnumerator Resume()
    {
        yield return new WaitForSeconds(1.2f);

        attacking = false;

        animator.SetTrigger("WalkLeft");
    }

    public float CalculateMoveTime()
    {
        //Calculates total distance from starting to end position
        var fullDistance = Vector2.Distance(leftWaypoint.position, rightWaypoint.position);

        //Calculate velocity of object given the full distance and move time
        var velocity = fullDistance / moveDuration;

        //Calculates how much further the object needs to move
        var remainingDistance = Vector2.Distance(transform.position, endPosition.position);

        //Calculate move time
        var time = remainingDistance / velocity;

        return time;
    }

    public bool CheckIfCanAttack(Transform player)
    {
        if(player.position.x < transform.position.x && _currentMoveDirection == MoveDirection.Left)
        {
            return true;
        }
        else if(player.position.x > transform.position.x && _currentMoveDirection == MoveDirection.Right)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}