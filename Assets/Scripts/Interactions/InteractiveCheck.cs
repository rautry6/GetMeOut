using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCheck : MonoBehaviour
{
    [SerializeField, Tooltip("The Interact Image GameObject")]
    private GameObject interactIcon;

    [SerializeField] private GMOEventType gmoEventType;

    [SerializeField, Header("Only Needs Set For DoorManager Checks")]
    private DoorManager doorManager;

    private VentManager _ventManager;

    private void Start()
    {
        _ventManager = GetComponentInParent<VentManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_ventManager != null)
            {
                if (_ventManager.IsInVentMovement) return;
                // VentManager only exists on vent game objects
                _ventManager.DetectedVent = gameObject;
            }

            //AutoSave.Instance.Save();
            interactIcon.SetActive(true);

            if (doorManager != null)
            {
                // Need to know which doorManager belongs to this game object
                ScreenTransition.UpdateCurrentDoorManager(doorManager);
            }


            var interactivePressedHandler = other.gameObject.GetComponentInChildren<HandleInteractionPressed>();
            if (interactivePressedHandler != null)
            {
                interactivePressedHandler.SetGMOEventType(gmoEventType);
                interactivePressedHandler.SetCanInteractTrue();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ScreenTransition.UpdateCurrentDoorManager(doorManager);
            if (_ventManager != null)
            {
                if (_ventManager.IsInVentMovement) return;
                _ventManager.DetectedVent = gameObject;
            }

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
            if (_ventManager != null)
            {
                _ventManager.DetectedVent = null;
            }

            interactIcon.SetActive(false);
            var interactivePressedHandler = other.gameObject.GetComponentInChildren<HandleInteractionPressed>();
            if (interactivePressedHandler != null)
            {
                interactivePressedHandler.SetCanInteractFalse();
            }
        }
    }
}