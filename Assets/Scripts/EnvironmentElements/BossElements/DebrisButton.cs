using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisButton : MonoBehaviour
{
    [SerializeField] private FallingDebris LinkedFallingDebris;
    private bool canTrigger = false;

    private void Awake()
    {
        canTrigger = true;
    }


    public void DropDebris()
    {
        if (canTrigger)
        {
            LinkedFallingDebris.Fall();

            gameObject.SetActive(false);
        }
    }
}
