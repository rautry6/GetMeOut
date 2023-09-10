using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private PlayerAnimations playerAnimations;
    private SpriteRenderer spriteRenderer;
    private Move playerMove;
    Rigidbody2D rb;

    [SerializeField]float _dashForce = 10f;
    [SerializeField] float _dashCooldown = 4f;
    float timer = 0f;
    bool canDash = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<Move>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = playerAnimations.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            canDash = false;
            timer = _dashCooldown;

            Vector2 direction = playerMove.Direction;

            if(direction == Vector2.zero)
            {
                if (spriteRenderer.flipX)
                {
                    direction = new Vector2(-1, 0);
                }
                else
                {
                    direction = new Vector2(1, 0);
                }
            }

            rb.AddForce(direction * _dashForce, ForceMode2D.Impulse);
        }

        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            canDash = true;
        }
    }
}
