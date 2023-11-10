using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    [SerializeField] private PlayerSFXManager playerSFXManager;
    public int healthPoints = 3;
    public int GetHealthPoints => healthPoints;

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
            StartCoroutine(HealthShake(healthUI[healthPoints - 1].gameObject));
            playerSFXManager.PlayHurtSFX();
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
        playerSFXManager.PlayHeartPickUpSFX();
        //Keeps index in bound for UI
        if (healthPoints < healthUI.Length)
        {
            healthUI[healthPoints].sprite = fullSprite;
        }

        healthPoints++;

        if (healthPoints > healthUI.Length)
        {
            healthPoints = healthUI.Length; //Keeps health from being above max health
        }
    }

    private IEnumerator HealthShake(GameObject objToShake)
    {
        HorizontalLayoutGroup hlg = objToShake.transform.parent.GetComponent<HorizontalLayoutGroup>();

        //Disable horizonal layout so piece can move
        if(hlg != null)
        {
            hlg.enabled = false;
        }

        Vector3 originalPosition = objToShake.transform.position;

        //How far left and right you want the piece to move
        float objShakeOffset = 20f;
        Vector3 shakeOffsetPosition = (Vector3.right * objShakeOffset);

        Vector3 targetPosition = objToShake.transform.position - shakeOffsetPosition;

        //How fast each individual shake occurs
        float shakeSpeed = 0.05f;

        //How many times the heart piece moves left and right
        int numberOfShakes = 4;

        for(int i = 0; i < numberOfShakes; i++)
        {
            objToShake.transform.DOMove(targetPosition, shakeSpeed);

            yield return new WaitForSeconds(shakeSpeed);

            shakeOffsetPosition = -shakeOffsetPosition;
            targetPosition = originalPosition + shakeOffsetPosition;

            objToShake.transform.DOMove(targetPosition, shakeSpeed);

            yield return new WaitForSeconds(shakeSpeed);

            shakeOffsetPosition = -shakeOffsetPosition;
            targetPosition = originalPosition + shakeOffsetPosition;
        }

        //Return to original position
        objToShake.transform.DOMove(originalPosition, shakeSpeed);

        yield return new WaitForSeconds(shakeSpeed);

        //Re-enable horizontal layout group
        if (hlg != null)
        {
            hlg.enabled = true;
        }


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") || collision.CompareTag("Trap"))
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

            if (healthPoints > 0 && !invulnerable)
            {
                pmove.ApplyKnockback(direction);
            }
        }

        //No knockback on spikes
        if(collision.CompareTag("Spike"))
        {
            TakeDamage();
        }

        if(collision.CompareTag("DeathPit"))
        {
            PlayerDeath.TriggerEvent();
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
        if (invulnerable) return;
        PlayerDeath.TriggerEvent(); 
    }
    
    public void StrictDie()
    {
        PlayerDeath.TriggerEvent(); 
    }

    
    public void Respawn()
    {
        transform.position = new Vector3(AutoSave.Instance.posX, AutoSave.Instance.posY, AutoSave.Instance.posZ);
        healthPoints = AutoSave.Instance.health;
        UpdateHeartsUI();
    }

    // button click or event trigger

    public void Restart()
    {
        AutoSave.Instance.ResetHealth();
        
    }

    public void ReportHealth()
    {
        AutoSave.Instance.health = healthPoints;
    }
    
    public void UpdateInvulnerable(bool isInvulnerable)
    {
        invulnerable = isInvulnerable;
    }

    private void UpdateHeartsUI()
    {
        foreach (var t in healthUI)
        {
            t.sprite = depletedSprite;
        }

        for (var i = 0; i <= healthPoints - 1; i++)
        {
            healthUI[i].sprite = fullSprite;
        }
    }
}
