using System;
using UnityEngine;

namespace Enemies
{
    public class MosquitoDetection : MonoBehaviour
    {
        public bool PlayerDetected;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerDetected = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerDetected = false;
            }
        }
    }
}