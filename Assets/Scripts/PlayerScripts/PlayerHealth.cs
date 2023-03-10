using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image[] healthUI;
    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite depletedSprite;

    [Header("Invunerability")]
    [SerializeField] private float invulTime = 1.5f;
    [SerializeField] private float flashSpeed = 0.1f;
    [SerializeField] private bool invulnerable = false;

    [Header("Sprite Renderer")]
    [SerializeField] private SpriteRenderer sr;

    [Header("Player Movement")]
    [SerializeField] private Move pmove;

    [Header("Player Death Event")]
    [SerializeField] private GameEvent PlayerDeath;

    private int healthPoints = 3;

    public void TakeDamage()
    {
        //Return out if the playerMove is invulnerable
        if (invulnerable)
        {
            return;
        }

        //Makes sure no negative index for UI
        if(healthPoints > 0)
        {
            healthUI[healthPoints-1].sprite = depletedSprite;
        }

        healthPoints--;

        if (healthPoints <= 0)
        {
            healthPoints = 0; //Keeps health from being negative
            Die();
        }
        else
        {
            //Makes playerMove invulnerable
            StartCoroutine(InvulTime());
        }

    }

    public void Heal()
    {
        //Keeps index in bound for UI
        if (healthPoints < healthUI.Length)
        {
            healthUI[healthPoints].sprite = fullSprite;
        }

        healthPoints++;

        if (healthPoints > healthUI.Length)
        {
            healthPoints = healthUI.Length - 1 ; //Keeps health from being negative
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Vector3 direction;

            if(collision.gameObject.transform.position.x < gameObject.transform.position.x)
            {
                direction = Vector3.right;
            }
            else if(collision.gameObject.transform.position.x > gameObject.transform.position.x)
            {
                direction = Vector3.left;
            }
            else
            {
                direction = Vector3.left;
            }

            TakeDamage();

            if (healthPoints > 0)
            {
                pmove.ApplyKnockback(direction);
            }
        }

        if(collision.tag == "Health")
        {
            Heal();
        }
    }

    public IEnumerator InvulTime()
    {
        invulnerable = true;

        StartCoroutine(Flash());

        TakeDamage();

        //Just tests heal function atm
        yield return new WaitForSeconds(invulTime);

        invulnerable = false;

        StopCoroutine(Flash());

        if (!sr.enabled)
        {
            sr.enabled = true;
        }
    }

    //Makes player sprite flash
    public IEnumerator Flash()
    {
        sr.enabled = false;

        yield return new WaitForSeconds(flashSpeed);

        sr.enabled = true;

        yield return new WaitForSeconds(flashSpeed);

        //Keeps running while the playerMove is invulnerable
        if (invulnerable)
        {
            StartCoroutine(Flash());
        }
    }

    public void Die()
    {
        PlayerDeath.TriggerEvent();
    }
}
