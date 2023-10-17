using System;
using System.Collections;
using System.Collections.Generic;
using DDA;
using UnityEngine;
using UnityEngine.Serialization;

public class MosquitoProjectile : MonoBehaviour
{
    [SerializeField] private float easyProjectileFlySpeed;
    [SerializeField] private float mediumProjectileFlySpeed;
    [SerializeField] private float hardProjectileFlySpeed;
    private Rigidbody2D _projectileRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _projectileRigidbody = GetComponent<Rigidbody2D>();
        var velocity = GetDifficultyBasedVelocity();
        Debug.Log(velocity);
        _projectileRigidbody.velocity = (Vector2) (transform.up * velocity);
        
    }

    private float GetDifficultyBasedVelocity()
    {
        switch (DDA.DDA.CurrentDifficulty)
        {
            case Difficulties.Easy: return easyProjectileFlySpeed;
            case Difficulties.Medium: return mediumProjectileFlySpeed;
            case Difficulties.Hard: return hardProjectileFlySpeed;
        }
        Debug.LogWarning("MosquitoProjectile.GetDifficultyBasedVelocity switch failed!");
        return mediumProjectileFlySpeed;
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
