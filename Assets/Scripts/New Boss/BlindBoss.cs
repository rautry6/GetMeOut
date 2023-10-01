using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class BlindBoss : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private BossStates currentState;

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


    [Header("Dashing")]

    [SerializeField, Tooltip("Range the player needs to be in before the boss will charge")]
    private float chargeRange = 2f;

    [SerializeField]
    private float maxYDifferentialForCharge = 10f;

    [SerializeField]
    private LayerMask wallLayer;

    private Vector2 chargeDirection;

    private float wallDetectionRange = 2f;


    [Header("Hit Debris")]

    private float health = 100f;


    private enum BossStates
    {
        Patroling,
        Dashing,
        DebrisLand, 
        Stunned,
    }


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
        currentState = BossStates.Patroling;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove) return;

        if (!moving && currentState == BossStates.Patroling)
        {
            moving = true;
            StartMovement();
            return;
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
