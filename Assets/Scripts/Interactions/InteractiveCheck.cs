using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCheck : MonoBehaviour
{
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private GMOEventType gmoEventType;
    [SerializeField] private DoorManager doorManager;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ScreenTransition.UpdateCurrentDoorManager(doorManager);
            interactIcon.SetActive(true);
            var interactivePressedHandler = other.gameObject.GetComponentInChildren<HandleInteractionPressed>();
            if (interactivePressedHandler != null)
            {
                interactivePressedHandler.SetGMOEventType(gmoEventType);
                interactivePressedHandler.SetCanInteractTrue();
                
            }
        }
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ScreenTransition.UpdateCurrentDoorManager(null);
            interactIcon.SetActive(false);
            var interactivePressedHandler = other.gameObject.GetComponentInChildren<HandleInteractionPressed>();
            if (interactivePressedHandler != null)
            {
                interactivePressedHandler.SetCanInteractFalse();
            }
        }
    }
}
