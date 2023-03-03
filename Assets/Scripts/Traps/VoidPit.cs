using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidPit : MonoBehaviour
{
    [SerializeField] private GameEvent death;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == " Player")
        {
            death.TriggerEvent();
        }
    }
}
