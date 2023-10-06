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
    private float moveSpeed = 90f;

    private bool moveRight = true;
    private bool moving = false;

    private bool canMove = false;
    


    [Header("Dashing")]

    [SerializeField, Tooltip("Range the player needs to be in before the boss will charge")]
    private float chargeRange = 2f;

    [SerializeField]
    private float maxYDifferentialForCharge = 10f;

    [SerializeField]
    private float chargePower = 5f;

    [SerializeField]
    private float dashCooldown = 3f;
    private float dashTimer = 0f;

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

    [SerializeField] float vel;
    [SerializeField] float totalDist;
    [SerializeField] float remainginDist;
    [SerializeField] float time;

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
                    Stun();
                }
            }
            else
            {
                //Checks if the boss hits a wall while charging
                var hit = Physics2D.Raycast(transform.position, Vector2.left, wallDetectionRange, wallLayer);
                Debug.DrawRay(transform.position, Vector2.left * wallDetectionRange, Color.yellow);
                if (hit.collider != null)
                    
                {
                    Stun();
                }
            }
        }

        if (!charging && dashTimer <= 0 && Mathf.Abs(transform.position.x - player.position.x) < chargeRange && Mathf.Abs(transform.position.y - player.position.y) < maxYDifferentialForCharge)
        {
            charging = true;
            currentState = BossStates.Charging;

            moving = false;
            DOTween.Kill(transform);

            float offset = 7f;

            currentChargePoint = new Vector2(moveRight == false ? player.position.x - offset :
                player.position.x + offset, 0f);

            StartCoroutine(MidStageDashAttack(offset));
        }

        if (!charging && dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }

    }


    IEnumerator MidStageDashAttack(float offset)
    {
        bossRigidBody.velocity = Vector2.zero;

        float chargeSpeed = 1f;
        bool finished = false;

        float totalDisance = Vector2.Distance(currentChargePoint, transform.position);
        float velocity = 15f;

        totalDist = totalDisance;
        vel = velocity;

        float remainingDistance = Vector2.Distance(currentChargePoint, transform.position);
        float moveTime = remainingDistance / velocity;

        remainginDist = remainingDistance;
        time = moveTime;

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

        remainginDist = remainingDistance;
        time = moveTime;

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

        remainginDist = remainingDistance;
        time = moveTime;

        CheckSpriteFlip();

        transform.DOMoveX(currentChargePoint.x, moveTime).SetEase(Ease.Linear).OnComplete(() => finished = true);

        yield return new WaitWhile(() => !finished);

        StartCoroutine(Idle());
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

    IEnumerator Idle()
    {
        dashTimer = dashCooldown;
        animator.SetBool("isCoolingDown", true);
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(idleTime);

        animator.SetBool("isCoolingDown", false);
        charging = false;

        currentState = BossStates.Patrolling;
    }

    public void Stun()
    {
        StopAllCoroutines();
        DOTween.Kill(transform, false);
        currentState = BossStates.Stunned;
        StartCoroutine(Stunned());
    }

    IEnumerator Stunned()
    {
        dashTimer = dashCooldown;
        animator.SetBool("isCoolingDown", true);
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(idleTime);

        animator.SetBool("isCoolingDown", false);
        charging = false;

        currentState = BossStates.Patrolling;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Debris"))
        {
            Stun();
        }
    }
}
