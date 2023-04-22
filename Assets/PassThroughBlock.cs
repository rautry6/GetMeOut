using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughBlock : MonoBehaviour
{
    private bool _canFall;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<PlatformEffector2D>().rotationalOffset = 0f;
            _canFall = true;
        }
    }

    private void Update()
    {
        if (_canFall)
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                gameObject.GetComponent<PlatformEffector2D>().rotationalOffset = 180f;
                _canFall = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<PlatformEffector2D>().rotationalOffset = 0f;
            _canFall = false;
        }
    }
}