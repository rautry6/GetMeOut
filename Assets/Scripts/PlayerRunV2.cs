using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunV2 : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private int maxNumberJumps;
    private float _movementInputDirection;
    private Rigidbody2D _playerRigidbody;
    private SpriteRenderer _playerSpriteRenderer;
    private Animator _playerAnimator;
    private bool _isFacingRight;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _canJump;
    private int _remainingJumps;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask wallLayer;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
        _remainingJumps = maxNumberJumps;
    }

    private void Update()
    {
        CheckInput();
        HandleMovementFacing();
        CheckIfCanJump();
    }
    
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckIfCanJump()
    {
        if (_isGrounded && _playerRigidbody.velocity.y <= 0)
        {
            _remainingJumps = maxNumberJumps;
        }

        if (_remainingJumps <= 0)
        {
            _canJump = false;
        }
        else
        {
            _canJump = true;
        }
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (_canJump)
        {
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpPower);
            _remainingJumps--;

        }
    }


    private void ApplyMovement()
    {
        _playerRigidbody.velocity = new Vector2(_movementInputDirection * movementSpeed, _playerRigidbody.velocity.y);
    }

    private void HandleMovementFacing()
    {
        AnimatorSetWalking();
        HandleSpriteRendererFlip();
    }

    private void HandleSpriteRendererFlip()
    {
        if (_isFacingRight && _movementInputDirection < 0) Flip();
        else if (!_isFacingRight && _movementInputDirection > 0) Flip();
    }

    private void AnimatorSetWalking()
    {
        if (_playerRigidbody.velocity.x != 0)
        {
            _playerAnimator.SetBool(IsWalking, true);
        }
        else
        {
            _playerAnimator.SetBool(IsWalking, false);
        }
    }

    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayerMask);
        _isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wallLayer);
        _isTouchingWall = Physics2D.Raycast(wallCheck.position, -transform.right, wallCheckDistance, wallLayer);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        _playerSpriteRenderer.flipX = !_isFacingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        var cachedWallCheckPosition = wallCheck.position;
        Gizmos.DrawLine(cachedWallCheckPosition, new Vector3(cachedWallCheckPosition.x + wallCheckDistance, cachedWallCheckPosition.y, cachedWallCheckPosition.z));
    }
}
