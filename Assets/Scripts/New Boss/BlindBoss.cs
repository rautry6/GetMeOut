using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class BlindBoss : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Patrolling")]
    [SerializeField, Tooltip("Left point goes at front of list")]
    private GameObject[] patrolPoints;

    [SerializeField]
    private float moveTime = 7f;

    private bool moveRight = true;
    private bool moving = false;

    private bool canMove = false;

    private float totalDistanceToTravel;

    private float bossVelocity;



    // Start is called before the first frame update
    void Start()
    {   
        totalDistanceToTravel = Mathf.Abs(patrolPoints[0].transform.position.x - patrolPoints[1].transform.position.x);
        bossVelocity = totalDistanceToTravel / moveTime;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = true;

        animator = GetComponent<Animator>();

        //Test code
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove) return;

        if (!moving)
        {
            moving = true;
            StartMovement();
        }
    }

    void StartMovement()
    {
        animator.SetBool("isWalking", true);

        float currentDistanceToTravel = moveRight == false ? Mathf.Abs(transform.position.x - patrolPoints[0].transform.position.x) : Mathf.Abs(transform.position.x - patrolPoints[1].transform.position.x);

        float timeToMove = currentDistanceToTravel / bossVelocity;

        transform.DOMoveX(moveRight == false ? patrolPoints[0].transform.position.x : patrolPoints[1].transform.position.x, timeToMove)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                moveRight = !moveRight;

                CheckSpriteFlip();

                moving = false;
            });

    }

    /// <summary>
    /// Flips the boss's sprite based on the direction they are supposed to be moving
    /// </summary>
    void CheckSpriteFlip()
    {
        if (moveRight)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void EnableMovement()
    {
        canMove = true;
    }
}
