using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SlimePlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {  
            GetComponentInParent<Slime>().OnSlimeDie();
            gameObject.SetActive(false);
        }
    }
}
