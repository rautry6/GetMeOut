using System;
using System.Collections;
using System.Collections.Generic;
using GetMeOut;
using GetMeOut.Checks;
using Unity.VisualScripting;
using UnityEngine;

namespace GetMeOut
{
    public class WallInteractor : MonoBehaviour
    {
        public bool WallJumping { get; private set; }

        [Header("Wall Slide"), SerializeField, Range(0.1f, 5f), Tooltip("Max speed player can slide down a wall")]
        float wallSlideMaxSpeed = 2f;

        [Header("Wall Jump"), SerializeField, Tooltip("Kind of wall jump to perform")]
        Vector2 wallJumpClimb = new Vector2(4f, 12f);
        
        [Header("Wall Bounce"), SerializeField, Tooltip("Kind of wall jump to perform")]
        Vector2 wallJumpBounce = new Vector2(10.7f, 10f);
        
        [Header("Wall Leap"), SerializeField, Tooltip("Kind of wall jump to perform")]
        Vector2 wallJumpLeap = new Vector2(14f, 12f);
        
        [SerializeField] private InputController controller;
        
        private CollisionDataRetrieving _collisionDataRetrieving;
        private Rigidbody2D _playerRigidbody;
        private Vector2 _velocity;
        private bool _onWall, _onGround, _tryingToJump;
        private float _wallDirectionX;

        private void Start()
        {
            _collisionDataRetrieving = GetComponent<CollisionDataRetrieving>();
            _playerRigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_onWall && !_onGround)
            {
                _tryingToJump |= controller.RetrieveJumpInput();
            }
        }

        private void FixedUpdate()
        {
            _velocity = _playerRigidbody.velocity;
            _onWall = _collisionDataRetrieving.OnWall;
            _onGround = _collisionDataRetrieving.OnGround;
            _wallDirectionX = _collisionDataRetrieving.ContactNormal.x;

            #region Wall Slide

            if (_onWall)
            {
                if (_velocity.y < -wallSlideMaxSpeed)
                {
                    _velocity.y = -wallSlideMaxSpeed;
                }
            }

            #endregion
            
            #region Wall Jump

            if ((_onWall && _velocity.x == 0) || _onGround)
            {
                WallJumping = false;
            }

            if (_tryingToJump)
            {
                if (-_wallDirectionX == controller.RetrieveMovementInput())
                {
                    _velocity = new Vector2(wallJumpClimb.x * _wallDirectionX, wallJumpClimb.y);
                    WallJumping = true;
                    _tryingToJump = false;
                } else if (controller.RetrieveMovementInput() == 0)
                {
                    _velocity = new Vector2(wallJumpBounce.x * _wallDirectionX, wallJumpBounce.y);
                    WallJumping = true;
                    _tryingToJump = false;
                }
                else
                {
                    _velocity = new Vector2(wallJumpLeap.x * _wallDirectionX, wallJumpLeap.y);
                    WallJumping = true;
                    _tryingToJump = false;
                }
            }
            
            #endregion

            _playerRigidbody.velocity = _velocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _collisionDataRetrieving.EvaluateCollision(other);
            if (_collisionDataRetrieving.OnWall && !_collisionDataRetrieving.OnGround && WallJumping)
            {
                _playerRigidbody.velocity = Vector2.zero;
            }
        }
    }
}