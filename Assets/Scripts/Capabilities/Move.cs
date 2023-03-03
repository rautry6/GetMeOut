using System;
using System.Collections;
using GetMeOut;
using UnityEngine;
using GetMeOut.Checks;
using System.Collections.Generic;

/// <summary>
/// This class is responsible for handling the players horizontal movement
/// </summary>
public class Move : MonoBehaviour
{
    [SerializeField] private InputController inputController = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxGroundAcc = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcc = 20f;
    [SerializeField, Range(0.05f, 0.5f)] private float wallStickTime = .25f;
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private bool canMove = true;
    [Header("Strings that must match exactly the animation they represent")]
    [SerializeField] private string playerRun = "Player_Run";
    [SerializeField] private string playerIdle = "Player_Idle";

    [Header("Knockback")]
    [SerializeField] private float horizontalKnockbackStrength = 5f;
    [SerializeField] private float verticalKnockbackStrength = 5f;
    [SerializeField] private Rigidbody2D playerRigidbody; 

    private Vector2 _direction;
    private Vector2 _desiredVelocity;
    private Vector2 _currentVelocity;
    private Rigidbody2D _playerRigidbody;
    private CollisionDataRetrieving _collisionDataRetrieving;
    private WallInteractor _wallInteractor;
    private float _maxSpeedChange;
    private float _acceleration;
    private bool _onGround;
    private float _wallStickCounter;


    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _collisionDataRetrieving = GetComponent<CollisionDataRetrieving>();
        _wallInteractor = GetComponent<WallInteractor>();
        _wallStickCounter = wallStickTime;
    }

    private void Update()
    {
        _direction.x = inputController.RetrieveMovementInput();
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(maxSpeed - _collisionDataRetrieving.Friction, 0f);
    }

    private void LateUpdate()
    {
        if(_onGround)
            RunningAnimationCheck();
    }

    private void RunningAnimationCheck()
    {
        if (_direction.x > 0f)
        {
            playerAnimations.ChangeAnimationState(AnimationState.RunningRight, playerRun);
        }

        else if (_direction.x < 0f)
        {
            playerAnimations.ChangeAnimationState(AnimationState.RunningLeft, playerRun);
        }
        else
        {
            playerAnimations.ChangeAnimationState(AnimationState.Idle, playerIdle);
        }
        
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        _onGround = _collisionDataRetrieving.OnGround;
        _currentVelocity = _playerRigidbody.velocity;
        _acceleration = _onGround ? maxGroundAcc : maxAirAcc;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _currentVelocity.x = Mathf.MoveTowards(_currentVelocity.x, _desiredVelocity.x, _maxSpeedChange);
        _playerRigidbody.velocity = _currentVelocity;

        #region Wall Stick

        if (_wallInteractor.HasWallInteractor)
        {
            if (_collisionDataRetrieving.OnWall && !_collisionDataRetrieving.OnGround && !_wallInteractor.WallJumping)
            {
                if (_wallStickCounter > 0f)
                {
                    _currentVelocity.x = 0f;

                    if (inputController.RetrieveMovementInput() == _collisionDataRetrieving.ContactNormal.x)
                    {
                        _wallStickCounter -= Time.deltaTime;
                    }
                    else
                    {
                        // reset wall stick counter
                        _wallStickCounter = wallStickTime;
                    }
                }
                else
                {
                    // reset wall stick counter
                    _wallStickCounter = wallStickTime;
                }
            }
        }

        #endregion
    }

    public void StopMovement()
    {
        canMove = false;
        _playerRigidbody.velocity = new Vector2(0, 0);
        _playerRigidbody.gravityScale = 0;
    }

    public void RegainMovement()
    {
        canMove = true;
        _playerRigidbody.gravityScale = 1;
    }

    public void ApplyKnockback(Vector3 direction)
    {
        playerRigidbody.AddForce(direction * horizontalKnockbackStrength, ForceMode2D.Impulse);
        playerRigidbody.AddForce(Vector3.up * verticalKnockbackStrength, ForceMode2D.Impulse);
    }

}
