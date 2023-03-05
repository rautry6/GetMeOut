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
    [SerializeField] private float maxJumpHeight = 6f;
    [SerializeField] private float jumpCooldown = 2f;

    [Header("Target Range")]
    [SerializeField, Tooltip("How close the player has to be before the enemy starts attacking")] private float targetRange = 50f;


    private Rigidbody2D rigidBody;
    private float GravityScale;
    private Transform player;

    [SerializeField]private bool calculating = false;
    [SerializeField] private bool _onGround = true;

    private CollisionDataRetrieving _collisionDataRetrieving;

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

        float velocityY = 10f;
        float velocityX = desiredDistance * gravity * 0.5f / velocityY;

        rigidBody.velocity = new Vector2(velocityX * -direction.x, velocityY);
        StartCoroutine(Cooldown());

    }

    public IEnumerator Cooldown()
    {
        _onGround = false;

        yield return new WaitForSeconds(0.5f);

        yield return new WaitUntil(() => _onGround);
        Debug.Log("grounCheck");


        yield return new WaitForSeconds(jumpCooldown);

        calculating = false;

        Debug.Log("done");
    }

}
