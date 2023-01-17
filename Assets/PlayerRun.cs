using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
     [SerializeField] private Animator playerAnimator;
        [SerializeField] private SpriteRenderer playerSpriteRenderer;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private Transform startPosition;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRayLength = 0.1f;
        [SerializeField] private Rigidbody2D playerRigidbody;
        [SerializeField] private float jumpForce;
        [SerializeField] private LayerMask groundLayer;
        private float _horizontal;
        private bool _doubleJump;
        private static readonly int HorizontalSpeed = Animator.StringToHash("horizontalSpeed");
        public bool CanMove { get; set; } = true;
    
        void Update()
        {
            if (!CanMove)
            {
                return;
            }
    
            if (IsGrounded() && !Input.GetButton("Jump"))
            {
                _doubleJump = false;
            }
            
            _horizontal = Input.GetAxisRaw("Horizontal");
            playerAnimator.SetFloat(HorizontalSpeed, Mathf.Abs(_horizontal));
            HandleSpriteFlip(_horizontal);
            if (Input.GetButtonDown("Jump"))
            {
                if (IsGrounded() || _doubleJump)
                {
                    playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
                    _doubleJump = !_doubleJump;
                }
            }
    
            if (Input.GetButtonUp("Jump") && playerRigidbody.velocity.y > 0f)
            {
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y * 0.5f);
            }
        }
    
        private void FixedUpdate()
        {
            playerRigidbody.velocity =
                new Vector2(_horizontal * moveSpeed * Time.fixedDeltaTime, playerRigidbody.velocity.y);
        }
    
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
                Debug.Log(foundGround);
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
