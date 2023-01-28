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
    [SerializeField] private bool isHorizontal;
    private bool _hasArrived = false;
    private MoveDirection _currentMoveDirection;

    private enum MoveDirection
    {
        Right,
        Left,
        Up,
        Down
    }
    private void Start()
    {
        _currentMoveDirection = isHorizontal ? MoveDirection.Right : MoveDirection.Up;
        if(_currentMoveDirection == MoveDirection.Right)
            MovePlatformRight();
        else
        {
            MovePlatformUp();
        }
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
            else if(_currentMoveDirection == MoveDirection.Right)
            {
                MovePlatformRight();
            }
            else if (_currentMoveDirection == MoveDirection.Up)
            {
                MovePlatformUp();
            }
            else
            {
                MovePlatformDown();
            }
        }
    }

    private void MovePlatformUp()
    {
        transform.DOMoveY(endPosition.position.y, moveDuration).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            _hasArrived = true;
            _currentMoveDirection = MoveDirection.Down;
        });
    }

    private void MovePlatformDown()
    {
        transform.DOMoveY(startPosition.position.y, moveDuration).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            _hasArrived = true;
            _currentMoveDirection = MoveDirection.Up;
        });
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var playerFound = other.gameObject.GetComponent<Move>();
        if (playerFound != null)
        {
            other.transform.parent = transform;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        var playerFound = other.gameObject.GetComponent<Move>();
        
        if (playerFound == null) return;
        
        other.transform.parent = playerFound.GetComponent<Rigidbody2D>().velocity.x != 0 ? null : transform;
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
