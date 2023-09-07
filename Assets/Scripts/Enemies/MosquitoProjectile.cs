using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoProjectile : MonoBehaviour
{
    [SerializeField] private float projectileFlySpeed;
    
    private Rigidbody2D _projectileRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _projectileRigidbody = GetComponent<Rigidbody2D>();
        _projectileRigidbody.velocity = transform.up * projectileFlySpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var tag = other.gameObject.tag;
        if(tag is "Player" or "Wall" or "Ground")
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth)
            {
                playerHealth.TakeDamage();
            }
            Destroy(gameObject);
        }
    }
}
