using System;
using UnityEngine;

namespace DefaultNamespace.Checks
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private float normalY = 0.9f;
        
            // Auto Properties with private setters
        public bool OnGround { get; private set; }
        public float Friction { get; private set; }

        private void OnCollisionEnter2D(Collision2D other)
        {
            EvaluateCollision(other);
            RetrieveFriction(other);
        }
        
        private void OnCollisionStay2D(Collision2D other)
        {
            EvaluateCollision(other);
            RetrieveFriction(other);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            ResetGroundFriction();
        }

        private void ResetGroundFriction()
        {
            OnGround = false;
            Friction = 0f;
        }

        void EvaluateCollision(Collision2D collision2D)
        {
            for (var i = 0; i < collision2D.contactCount; i++)
            {
                Vector2 normal = collision2D.GetContact(i).normal;
                // Bitwise OR assignment
                OnGround |= normal.y >= normalY;
            }
        }

        void RetrieveFriction(Collision2D collision2D)
        {
            var material = collision2D.rigidbody.sharedMaterial;
            Friction = 0;
            if (material != null)
            {
                Friction = material.friction;
            }
        }
    }
}