using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingBat : MonoBehaviour
{
    [SerializeField, Tooltip("How far to the side of the player the bat stays")] private float latchDistance = 10f;
    [SerializeField] private GameObject player;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float attackRange = 30f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float chargeTime = 2f;
    [SerializeField] private float chargeCooldown = 6f;

    [SerializeField] private CapsuleCollider2D hitBox;
    [SerializeField] private Animator animator;

    private bool onRightSide = false;
    private bool dashing = false;
    private bool cooldownStarted = false;
    private bool charging = false;
    private float defaultAnimatorSpeed;


    [SerializeField]private States currentState;
    private Vector2 originalPosition;

    public enum States
    {
        Latched,
        Charging,
        Dashing,
        Waiting,
        Returning,
        Latching
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = States.Waiting;
        originalPosition = transform.position;
        DisableHitBox();
        defaultAnimatorSpeed = animator.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, originalPosition) > attackRange && !dashing)
        {
            currentState = States.Returning;
            StopAll();
        }

        switch (currentState)
        {
            case States.Charging:
                {
                    FollowPlayer();

                    if (!charging)
                    {
                        charging = true;
                        StartCoroutine(Charge());
                    }
                    break;
                }
            case States.Dashing:
                {
                    if (!dashing)
                    {
                        dashing = true;
                        StartCoroutine(Dash());
                    }
                    break;
                }
            case States.Waiting:
                {
                    if (Vector2.Distance(player.transform.position, originalPosition) < attackRange)
                    {
                        currentState = States.Latching;
                    }

                    break;
                }
            case States.Latching:
                {
                    if (!cooldownStarted)
                    {
                        cooldownStarted = true;
                        StartCoroutine(ChargeCooldown());
                    }

                    FollowPlayer();

                    break;
                }
            case States.Returning:
                {
                    Return();

                    //Checks if the player goes back into the attack range and the latch position is not outside the attack range
                    if (Vector2.Distance(player.transform.position, originalPosition) < attackRange)
                    {
                        switch (onRightSide)
                        {
                            case true:
                                {
                                    Vector2 targetLatchPoint = new Vector2(player.transform.position.x + latchDistance, player.transform.position.y);
                                    if (Vector2.Distance(originalPosition, targetLatchPoint) > attackRange)
                                    {
                                        return;
                                    }
                                   
                                    break;
                                }

                            case false:
                                {
                                    Vector2 targetLatchPoint = new Vector2(player.transform.position.x - latchDistance, player.transform.position.y);
                                    if (Vector2.Distance(originalPosition, targetLatchPoint) > attackRange)
                                    {
                                        return;
                                    }
                                   
                                    break;
                                }
                        }

                        currentState = States.Latching;
                    }

                    break;
                }
            }
    }

    public void DisableHitBox()
    {
        hitBox.enabled = false;
    }

    public void EnableHitBox()
    {
        hitBox.enabled = true;
    }

    public IEnumerator Dash()
    {
        EnableHitBox();
        float targetPosition;

        if (onRightSide)
        {
            targetPosition = player.transform.position.x - dashDistance;

            while(transform.position.x > targetPosition)
            {
                transform.position -= transform.right * dashSpeed * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            targetPosition = player.transform.position.x + dashDistance;

            while (transform.position.x < targetPosition)
            {
                transform.position += transform.right * dashSpeed * Time.deltaTime;
                yield return null;
            }
        }

        DisableHitBox();
        onRightSide = !onRightSide;
        currentState = States.Latching;
        dashing = false;
    
    }

    public IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(chargeCooldown);

        currentState = States.Charging;
        cooldownStarted = false;
    }

    public IEnumerator Charge()
    {
        animator.speed = 3f;

        yield return new WaitForSeconds(chargeTime);

        animator.speed = defaultAnimatorSpeed;
        currentState = States.Dashing;
        charging = false;
    }

    public void FollowPlayer()
    {
        switch (onRightSide)
        {
            case true:
                if (transform.position.x < player.transform.position.x + latchDistance)
                {
                    transform.position += transform.right * moveSpeed * Time.deltaTime;
                }
                else if (transform.position.x > player.transform.position.x + latchDistance)
                {
                    transform.position -= transform.right * moveSpeed * Time.deltaTime;
                }
                break;

            case false:
                if (transform.position.x > player.transform.position.x - latchDistance)
                {
                    transform.position -= transform.right * moveSpeed * Time.deltaTime;
                }
                else if (transform.position.x < player.transform.position.x - latchDistance)
                {
                    transform.position += transform.right * moveSpeed * Time.deltaTime;
                }
                break;
        }

        if (transform.position.y > player.transform.position.y)
        {
            transform.position -= transform.up * moveSpeed * Time.deltaTime;
        }
        else if (transform.position.y < player.transform.position.y)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
        }
    }

    public void Return()
    {
        if (transform.position.x < originalPosition.x)
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }
        else if (transform.position.x > originalPosition.x)
        {
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
        }

        if (transform.position.y > originalPosition.y)
        {
            transform.position -= transform.up * moveSpeed * Time.deltaTime;
        }
        else if (transform.position.y < originalPosition.y)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
        }

        if(transform.position.x == originalPosition.x && transform.position.y == originalPosition.y)
        {
            currentState = States.Waiting;
        }
    }

    public void StopAll()
    {
        StopAllCoroutines();
        charging = false;
        dashing = false;
        cooldownStarted = false;
    }
}
