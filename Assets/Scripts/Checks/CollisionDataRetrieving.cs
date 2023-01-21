using System;
using UnityEngine;

namespace GetMeOut.Checks
{
    public class CollisionDataRetrieving : MonoBehaviour
    {
        // Auto Properties with private setters
        public Vector2 ContactNormal { get; private set; }
        public bool OnGround { get; private set; }
        public bool OnWall { get; private set; }
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
            OnWall = false;
        }

        public void EvaluateCollision(Collision2D collision2D)
        {
            for (var i = 0; i < collision2D.contactCount; i++)
            {
                ContactNormal = collision2D.GetContact(i).normal;
                // Bitwise OR assignment
                OnGround |= ContactNormal.y >= 0.9f;
                OnWall = Mathf.Abs(ContactNormal.x) >= 0.9f;
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