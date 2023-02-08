using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image[] healthUI;
    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite depletedSprite;

    private int healthPoints = 3;

    private void Awake()
    {
        TakeDamage();
    }

    public void TakeDamage()
    {
        //Makes sure no negative index for UI
        if(healthPoints > 0)
        {
            healthUI[healthPoints-1].sprite = depletedSprite;
        }

        healthPoints--;

        if (healthPoints <= 0)
        {
            healthPoints = 0; //Keeps health from being negative
            //Do die
            //stuff
        }
        else
        {
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
            //Do die stuff
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            TakeDamage();
        }

        if(collision.tag == "Health")
        {
            Heal();
        }
    }

    public IEnumerator InvulTime()
    {

        //Just tests heal function atm
        yield return new WaitForSeconds(3f);

        Heal();
    }
}
