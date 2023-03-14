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
    [SerializeField] private SpriteRenderer spriteRenderer;

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
            spriteRenderer.flipX = false;
            MoveLeft();
        }
        else
        {
            spriteRenderer.flipX = true;
            MoveRight();
        }
    }

    public void MoveRight()
    {
        if (transform != null)
        {
            transform.DOMoveX(rightWaypoint.position.x, moveDuration).SetEase(easeType).OnComplete(() =>
            {
                StartCoroutine(Idle());
            });
        }
    }

    public void MoveLeft()
    {
        if (transform != null)
        {
            transform.DOMoveX(leftWaypoint.position.x, moveDuration).SetEase(easeType).OnComplete(() =>
            {
                StartCoroutine(Idle());
            });
        }
    }

    public IEnumerator Idle()
    {
        yield return new WaitForSeconds(idleTime);

        if (_currentMoveDirection == MoveDirection.Left)
        {
            spriteRenderer.flipX = true;
            _currentMoveDirection = MoveDirection.Right;
            MoveRight();
        }
        else
        {
            spriteRenderer.flipX = false;
            _currentMoveDirection = MoveDirection.Left;
            MoveLeft();
        }
    }
}