using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(Rigidbody2D))]
public class BlindBoss : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D bossRigidBody;

    private BossStates currentState;

    [SerializeField]
    private TMP_Text stateText;

    [SerializeField]
    private Transform player;


    [Header("Patrolling")]
    [SerializeField, Tooltip("Left point goes at front of list")]
    private GameObject[] patrolPoints;

    [SerializeField]
    private float moveSpeed = 100f;

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
    private float chargePower = 5f;

    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField]
    private float wallDetectionRange = 2f;

    private bool charging = false;

    private Vector2 currentChargePoint;

    [Header("Hit Debris")]

    private float health = 100f;


    private bool coolingDown = false;
    private float idleTime = 3f;

    private enum BossStates
    {
        Patrolling,
        Charging,
        DebrisLand, 
        Stunned,
    }


    // Start is called before the first frame update
    void Start()
    {   
        totalDistanceToTravel = Mathf.Abs(patrolPoints[0].transform.position.x - patrolPoints[1].transform.position.x);

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = true;

        animator = GetComponent<Animator>();

        bossRigidBody = GetComponent<Rigidbody2D>();

        //Test code
        canMove = true;
        currentState = BossStates.Patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove) return;

        stateText.text = currentState.ToString();

        if (!moving && currentState == BossStates.Patrolling)
        {
            moving = true;
            animator.SetBool("isWalking", true);
            return;
        }

        if (moving)
        {
            if (moveRight)
            {
                if(transform.position.x >= patrolPoints[1].transform.position.x)
                {
                    moveRight = false;
                    CheckSpriteFlip();
                    bossRigidBody.velocity = Vector3.zero;
                    return;
                }

                bossRigidBody.velocity = Vector2.right * moveSpeed;
            }
            else
            {
                if (transform.position.x <= patrolPoints[0].transform.position.x)
                {
                    moveRight = true;
                    CheckSpriteFlip();
                    bossRigidBody.velocity = Vector3.zero;
                    return;
                }

                bossRigidBody.velocity = Vector2.left * moveSpeed;
            }
        }
        Debug.DrawRay(transform.position, Vector2.right * wallDetectionRange, Color.yellow);
        if (currentState == BossStates.Charging)
        {
            if(moveRight)
            {
                //Checks if the boss hits a wall while charging
                var hit = Physics2D.Raycast(transform.position, Vector2.right, wallDetectionRange, wallLayer);
                Debug.DrawRay(transform.position, Vector2.right * wallDetectionRange, Color.yellow);
                if (hit.collider != null)

                {
                    StopAllCoroutines();
                    DOTween.Kill(transform, false);
                    currentState = BossStates.Stunned;
                    //StartCoroutine(CoolDown());
                }
            }
            else
            {
                //Checks if the boss hits a wall while charging
                var hit = Physics2D.Raycast(transform.position, Vector2.left, wallDetectionRange, wallLayer);
                Debug.DrawRay(transform.position, Vector2.left * wallDetectionRange, Color.yellow);
                if (hit.collider != null)
                    
                {
                    StopAllCoroutines();
                    DOTween.Kill(transform, false);
                    currentState = BossStates.Stunned;
                    //StartCoroutine(CoolDown());
                }
            }
        }

    }

    private void FixedUpdate()
    {
        if (!charging && Mathf.Abs(transform.position.x - player.position.x) < chargeRange && Mathf.Abs(transform.position.y - player.position.y) < maxYDifferentialForCharge)
        {
            charging = true;
            currentState = BossStates.Charging;

            moving = false;
            DOTween.Kill(transform);

            float offset = 5f;

            currentChargePoint = new Vector2(moveRight == false ? player.position.x - offset :
                player.position.x  + offset, 0f) ;

            StartCoroutine(MidStageDashAttack(offset));

            //Charge();
        }

        
    }

    IEnumerator MidStageDashAttack(float offset)
    {
        bossRigidBody.velocity = Vector2.zero;

        float chargeSpeed = 1f;
        bool finished = false;

        float totalDisance = Mathf.Abs(transform.position.x - currentChargePoint.x);
        float velocity = totalDisance / chargeSpeed;

        float remainingDistance = Mathf.Abs(transform.position.x - currentChargePoint.x);
        float moveTime = remainingDistance / velocity;

        CheckSpriteFlip();

        transform.DOMoveX(currentChargePoint.x, moveTime).SetEase(Ease.Linear).OnComplete(() => finished = true);

        yield return new WaitWhile(() => !finished);

        finished = false;

        moveRight = !moveRight;

        if (moveRight && player.position.x < transform.position.x)
        {
            moveRight = false;
        }
        else if (!moveRight && player.position.x > transform.position.x)
        {
            moveRight = true;
        }

        currentChargePoint = new Vector2(moveRight == false ? player.position.x - offset :
               player.position.x + offset, 0f);

         remainingDistance = Mathf.Abs(transform.position.x - currentChargePoint.x);
         moveTime = remainingDistance / velocity;

        CheckSpriteFlip();

        transform.DOMoveX(currentChargePoint.x, moveTime).SetEase(Ease.Linear).OnComplete(() => finished = true);

        yield return new WaitWhile(() => !finished);

        finished = false;

        if(moveRight && player.position.x < transform.position.x)
        {
            moveRight = false;
        }
        else if(!moveRight && player.position.x > transform.position.x)
        {
            moveRight = true;
        }


        currentChargePoint = new Vector2(moveRight == false ? player.position.x - offset :
                  player.position.x + offset, 0f);

         remainingDistance = Mathf.Abs(transform.position.x - currentChargePoint.x);
         moveTime = remainingDistance / velocity;

        CheckSpriteFlip();

        transform.DOMoveX(currentChargePoint.x, moveTime).SetEase(Ease.Linear).OnComplete(() => finished = true);

        yield return new WaitWhile(() => !finished);

        StartCoroutine(Idle());
    }

    void StartMovement()
    {
        animator.SetBool("isWalking", true);



        /*transform.DOMoveX(moveRight == false ? patrolPoints[0].transform.position.x : patrolPoints[1].transform.position.x, timeToMove)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                moveRight = !moveRight;

                CheckSpriteFlip();

                moving = false;
            }); */

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

    void Charge()
    {
        Debug.Log("Charging");
        Vector2 force;

        if(player.position.x < transform.position.x)
        {
            force = Vector2.left;
        }
        else
        {
            force = Vector2.right;
        } 



        bossRigidBody.AddForce(force * chargePower, ForceMode2D.Impulse);



        StartCoroutine(Idle());
    }

    IEnumerator Idle()
    {

        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(idleTime);

        charging = false;

        currentState = BossStates.Patrolling;
    }
}
