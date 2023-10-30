using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FallingDebris : MonoBehaviour
{
    Rigidbody2D debrisRigidbody;
    [SerializeField] private Collider2D collider1;
    [SerializeField] private Sprite destroyedSprite;
    [SerializeField] private BlindBoss boss;
    [SerializeField] private GameEvent debrisEvent;
    [SerializeField] private bool isLastDebris;

    public bool OnGround;
    public bool BossHit;
    
    // Start is called before the first frame update
    void Start()
    {
        debrisRigidbody = GetComponent<Rigidbody2D>();
        collider1 = GetComponent<Collider2D>();
    }
    
    private void Update()
    {
        if (OnGround && BossHit)
        {
            collider1.enabled = false;
            this.enabled = false;
        }    
    }
    
    public void Fall()
    {
        debrisRigidbody.gravityScale = 1;
    }

    public void Destroy()
    {
        if (isLastDebris)
        {
            debrisEvent.TriggerEvent();
        }
        BossHit = true;
        boss.TakeDamage(33.5f);
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = destroyedSprite;
        spriteRenderer.DOFade(0, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BlindBoss>() != null && !BossHit)
        {
            Destroy();
        }

        if (collision.CompareTag("Ground"))
        {
            OnGround = true;
            //Makes trap stick into floor
            debrisRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            //collider1.enabled = false;

            if (!BossHit)
            {
                boss.ChargeDebris(gameObject);
            }
        }
    }
}
