using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GetMeOut;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private float moveDuration;
    private bool _hasArrived = false;
    private MoveDirection _currentMoveDirection = MoveDirection.Right;

    private delegate void _currentMovementFunction();

    private enum MoveDirection
    {
        Right,
        Left
    }
    private void Start()
    {
        MovePlatformRight();
    }

    private void MovePlatformRight()
    {
        transform.DOMoveX(endPosition.position.x, moveDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            _hasArrived = true;
            _currentMoveDirection = MoveDirection.Left;
        });
    }
    
    private void MovePlatformLeft()
    {
        transform.DOMoveX(startPosition.position.x, moveDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            _hasArrived = true;
            _currentMoveDirection = MoveDirection.Right;
        });
    }

    private void Update()
    {
        if (_hasArrived)
        {
            _hasArrived = false;
            if (_currentMoveDirection == MoveDirection.Left)
            {
                MovePlatformLeft();
            }
            else
            {
                MovePlatformRight();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var playerFound = other.gameObject.GetComponent<Move>();
        if (playerFound != null)
        {
            other.transform.parent = transform;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        var playerFound = other.gameObject.GetComponent<Move>();
        if (playerFound != null)
        {
            other.transform.parent = null;
        }
    }
}
