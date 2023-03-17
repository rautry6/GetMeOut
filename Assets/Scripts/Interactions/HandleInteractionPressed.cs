using System;
using UnityEngine;



public class HandleInteractionPressed : MonoBehaviour
{
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private GameEvent levelTransitionEvent;
    [SerializeField] private GameEvent ventTraversalEvent;
    [SerializeField] private GameEvent powerUpEvent;
    [SerializeField] private GameEvent pcInteractionEvent;
    
    private GMOEventType _gmoEventType = GMOEventType.Empty;

    public void SetGMOEventType(GMOEventType eventType)
    {
        _gmoEventType = eventType;
    }
    public bool CanInteract { get; private set; }

    public void SetCanInteractTrue()
    {
        CanInteract = true;
    }
    
    public void SetCanInteractFalse()
    {
        CanInteract = false;
    }
    
    private void Update()
    {
        if (CanInteract)
        {
            interactIcon.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                SetCanInteractFalse();
                Debug.Log("pressed interact");
                HandleEvent();
            }
        }
    }

    private void HandleEvent()
    {
        switch (_gmoEventType)
        {
            case GMOEventType.Empty:
            {
                throw new Exception("Need to be sure the GMOEventType is not empty!");
            }
            case GMOEventType.LevelTransition:
            {
                levelTransitionEvent.TriggerEvent();
                
                break;
            }
            case GMOEventType.Vent:
            {
                ventTraversalEvent.TriggerEvent();
                break;
            }
            case GMOEventType.PowerUp:
            {
                powerUpEvent.TriggerEvent();
                break;
            }
            case GMOEventType.PCInteract:
            {
                pcInteractionEvent.TriggerEvent();
                break;
            }
            default: throw new NotImplementedException();
        }
    }
}
