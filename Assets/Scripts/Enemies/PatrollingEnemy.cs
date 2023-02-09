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

    private enum MoveDirection
    {
        Right,
        Left
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(_currentMoveDirection == MoveDirection.Left)
        {
            MoveLeft();
        }
        else
        {
            MoveRight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveRight()
    {
        transform.DOMoveX(rightWaypoint.position.x, moveDuration).SetEase(easeType).OnComplete(() =>
        {
            StartCoroutine(Idle());
        });
    }

    public void MoveLeft()
    {
        transform.DOMoveX(leftWaypoint.position.x, moveDuration).SetEase(easeType).OnComplete(() =>
        {
            StartCoroutine(Idle());
        });
    }

    public IEnumerator Idle()
    {
        yield return new WaitForSeconds(idleTime);

        if(_currentMoveDirection == MoveDirection.Left)
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
