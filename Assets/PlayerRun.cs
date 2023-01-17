using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    [Header("Assignable References")]
    [SerializeField] private Transform startPosition;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    
    [Header("Adjustable Variables")]
    [SerializeField] private float groundCheckRayLength = 0.1f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpPressedRememberTime = 0.2f;
    [SerializeField] private float groundedRememberTime = 0.2f;
    [Range(0, 1)]
    [SerializeField] private float horizontalDamping;
    [Range(0, 1)] [SerializeField] private float doubleJumpModifier;
    
    // Member Variables
    private float _horizontal;
    private bool _doubleJump;
    private static readonly int HorizontalSpeed = Animator.StringToHash("horizontalSpeed"); // efficiency recommendation
    private float _jumpPressedRemember = 0;
    private float _groundedRemember = 0;

    // Properties
    public bool CanMove { get; set; } = true;

    void Update()
    {
        _jumpPressedRemember -= Time.deltaTime;
        
        if (!CanMove)
        {
            Debug.Log("Hello");
            // playerRigidbody.velocity = Vector2.zero;
            playerAnimator.SetFloat(HorizontalSpeed, 0f);
            return;
        }

        if (IsGrounded())
        {
            _groundedRemember = groundedRememberTime;
        }

        // On the ground and no longer holding down the jump button be sure to set double jump back to false
        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            _doubleJump = false;
        }
        
        // horizontal input and idle run animation logic
        _horizontal = Input.GetAxisRaw("Horizontal");
        var playerCurrentHorizontalVelocity = playerRigidbody.velocity.x;
        playerCurrentHorizontalVelocity += _horizontal;
        playerCurrentHorizontalVelocity *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);
        playerRigidbody.velocity = new Vector2(playerCurrentHorizontalVelocity, playerRigidbody.velocity.y);
        playerAnimator.SetFloat(HorizontalSpeed, Mathf.Abs(_horizontal));
        HandleSpriteFlip(_horizontal);
        
        // first jump
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressedRemember = jumpPressedRememberTime;
            // need to be on the ground or be jumping with the ability to double jump
            if (_jumpPressedRemember > 0 && /*IsGrounded()*/ _groundedRemember > 0 || _doubleJump)
            {
                _jumpPressedRemember = 0f;
                _groundedRemember = 0f;
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
                
                // switch double jump to be the opposite of what it was to be sure to be able to double jump
                _doubleJump = !_doubleJump;
            }
        }
    
        // if the players y velocity is above 0 then the player is in the middle of a jump and should double jump - need to introduce variable for the second jump power modifier
        if (Input.GetButtonUp("Jump") && playerRigidbody.velocity.y > 0f)
        {
            var playerVelocity = playerRigidbody.velocity;
            playerVelocity = new Vector2(playerVelocity.x, playerVelocity.y * doubleJumpModifier);
            playerRigidbody.velocity = playerVelocity;
        }
    }

    /*private void FixedUpdate()
    {
        playerRigidbody.velocity =
            new Vector2(_playerCurrentHorizontalVelocity * moveSpeed * Time.fixedDeltaTime, playerRigidbody.velocity.y);
    }*/

    private void HandleSpriteFlip(float horizontal)
    {
        if (horizontal < 0)
        {
            playerSpriteRenderer.flipX = true;
        }
        else if (horizontal > 0)
        {
            playerSpriteRenderer.flipX = false;
        }
    }

    private bool IsGrounded()
    {
        var groundCheckHit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRayLength, groundLayer);
        if (groundCheckHit.collider != null)
        {
            var foundGround = groundCheckHit.collider.gameObject.GetComponent<Ground>();
            if (foundGround != null)
            {
                return true;
            }
        }

        return false;
    }

    public void ResetToStart()
    {
        transform.position = startPosition.position;
    }
}
