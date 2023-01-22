using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerAI : MonoBehaviour
{
    [SerializeField] private float acceleration = 50.0f;
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float maxSpeedChange = 0f;
    [SerializeField] private float maxAcceleration = 100f;
    [SerializeField] private float minAcceleration = 10.0f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int direction = 1;
    [SerializeField] private Transform player;
    [SerializeField] private bool canJump = true;
    [SerializeField] private float lastDistanceToPlayer = 0f;
    [SerializeField] private float distance = 0f;
    Vector2 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector2.Distance(transform.position, player.position);

        if(player.position.x < transform.position.x)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        if(lastDistanceToPlayer < distance)
        {
            lastDistanceToPlayer = distance;
            if(acceleration < maxAcceleration)
            {
                acceleration+= 5;
            }
        }
        else if (lastDistanceToPlayer > distance)
        {
            lastDistanceToPlayer = distance;
            if (acceleration > minAcceleration)
            {
               acceleration-= 5;
            }
        }

        maxSpeedChange = acceleration * Time.deltaTime;

    }

    private void FixedUpdate()
    {
        velocity = new Vector2(direction, 0f);
        Vector2 desiredVelocity = velocity * maxSpeed;
        velocity = rb.velocity;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        if(player.position.y > (transform.position.y +1) && canJump)
        {
            canJump = false;
            //Jump();
        }

        rb.velocity = velocity;
    }

    public void Jump()
    {
        velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            canJump |= normal.y >= 0.9f;
        }
    }
}
