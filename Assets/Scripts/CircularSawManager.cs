using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CircularSawManager : MonoBehaviour
{
    [SerializeField] private CircularSawBehavior startingBehavior;
    [SerializeField] private MoveDirection startingMoveDirection;
    [SerializeField] private float moveDuration;
    [SerializeField] private Transform UpTargetTransform;
    [SerializeField] private Transform DownTargetTransform;
    [SerializeField] private Transform LeftTargetTransform;
    [SerializeField] private Transform RightTargetTransform;
    [SerializeField] private GameObject sawGameObject;
    [SerializeField] private Ease easeType;

    private CircularSawBehavior _currentBehavior;
    private MoveDirection _currentMoveDirection;
    private bool isTweening;

    private enum CircularSawBehavior
    {
        Idle,
        Moving,
    }

    private enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    private void Start()
    {
        _currentBehavior = startingBehavior;
        _currentMoveDirection = startingMoveDirection;
    }

    private void Update()
    {
        HandleCurrentBehavior();
    }

    private void HandleCurrentBehavior()
    {
        switch (_currentBehavior)
        {
            case CircularSawBehavior.Idle:
            {
                Debug.Log(_currentBehavior.ToString());
                break;
            }
            case CircularSawBehavior.Moving:
            {
                HandleMovingInDirection();
                break;
            }
        }
    }

    private void HandleMovingInDirection()
    {
        if (isTweening) return; 
        
        switch (_currentMoveDirection)
        {
            case MoveDirection.Up:
            {
                if (!CheckIfArrivedAtTargetPosition(UpTargetTransform))
                {
                    isTweening = true;
                    sawGameObject.transform.DOMoveY(UpTargetTransform.position.y, moveDuration).SetEase(easeType).OnComplete(() =>
                    {
                        isTweening = false;
                    });
                }
                else
                {
                    UpdateSawDirection(MoveDirection.Down);
                }

                break;
            }
            case MoveDirection.Down:
            {
                if (!CheckIfArrivedAtTargetPosition(DownTargetTransform))
                {
                    isTweening = true;
                    sawGameObject.transform.DOMoveY(DownTargetTransform.position.y, moveDuration).SetEase(easeType).OnComplete(() =>
                    {
                        isTweening = false;
                    });
                }
                else
                {
                    UpdateSawDirection(MoveDirection.Up);
                }

                break;
            }
            case MoveDirection.Left:
            {
                if (!CheckIfArrivedAtTargetPosition(LeftTargetTransform))
                {
                    isTweening = true;
                    sawGameObject.transform.DOMoveX(LeftTargetTransform.position.x, moveDuration).SetEase(easeType).OnComplete(
                        () =>
                        {
                            isTweening = false;
                        });
                }
                else
                {
                    UpdateSawDirection(MoveDirection.Right);
                }

                break;
            }
            case MoveDirection.Right:
            {
                if (!CheckIfArrivedAtTargetPosition(RightTargetTransform))
                {
                    isTweening = true;
                    sawGameObject.transform.DOMoveX(RightTargetTransform.position.x, moveDuration).SetEase(easeType).OnComplete(
                        () =>
                        {
                            isTweening = false;
                        });
                }
                else
                {
                    UpdateSawDirection(MoveDirection.Left);
                }

                break;
            }
        }
    }

    private bool CheckIfArrivedAtTargetPosition(Transform targetPosition)
    {
        return Vector3.Distance(sawGameObject.transform.position, targetPosition.position) < .25f;
    }

    private void UpdateSawBehavior(CircularSawBehavior newSawBehavior)
    {
        _currentBehavior = newSawBehavior;
    }

    private void UpdateSawDirection(MoveDirection moveDirection)
    {
        _currentMoveDirection = moveDirection;
    }
}