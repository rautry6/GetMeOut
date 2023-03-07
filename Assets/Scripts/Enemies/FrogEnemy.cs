using GetMeOut.Checks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Member;

[RequireComponent(typeof(Rigidbody2D))]
public class FrogEnemy : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float maxJumpDistance = 10f;
    [SerializeField] private float JumpHeight = 10f;
    [SerializeField] private float jumpCooldown = 2f;

    private float jumpVariability = 1f;


    [Header("Target Range")]
    [SerializeField, Tooltip("How close the player has to be before the enemy starts attacking")] private float targetRange = 50f;


    private Rigidbody2D rigidBody;
    private float GravityScale;
    private Transform player;

    [SerializeField]private bool calculating = false;
    [SerializeField] private bool _onGround = true;

    private CollisionDataRetrieving _collisionDataRetrieving;

    [SerializeField] private Collider2D col;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        GravityScale = rigidBody.gravityScale;
        player = GameObject.Find("Player").transform;
        _collisionDataRetrieving = GetComponent<CollisionDataRetrieving>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if player is in range and not already calculating
        if(Mathf.Abs(player.position.x - transform.position.x) < targetRange && !calculating)
        {
            StopAllCoroutines();
            calculating = true;

            CalculateForces();
        }
    }

    private void FixedUpdate()
    {
        _onGround = _collisionDataRetrieving.OnGround;
    }

    public void CalculateForces()
    {
        float gravity = Physics2D.gravity.magnitude;

        //Calculates distance and direction to player
        var heading = transform.position - player.position;
        var distance = heading.magnitude;
        var direction = heading / distance;

        //Determines how far the enemy is going to try and jump
        float desiredDistance = Mathf.Min(maxJumpDistance, Mathf.Abs(distance));

        float velocityY = JumpHeight;


        float velocityX = desiredDistance * gravity * 0.5f / velocityY;

        rigidBody.velocity = new Vector2(velocityX * -direction.x, velocityY);
        StartCoroutine(Cooldown());

    }

    public IEnumerator Cooldown()
    {
        _onGround = false;

        yield return new WaitForSeconds(0.5f);

        yield return new WaitUntil(() => _onGround);

        float newJumpCooldown = Random.Range(jumpCooldown - jumpVariability, jumpCooldown + jumpVariability);

        yield return new WaitForSeconds(newJumpCooldown);

        calculating = false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, col);
        }
    }

}
