using System;
using Unity.VisualScripting;
using UnityEngine;



public class HandleInteractionPressed : MonoBehaviour
{
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private GameEvent levelTransitionEvent;
    [SerializeField] private GameEvent ventTraversalEvent;
    [SerializeField] private GameEvent powerUpEvent;
    [SerializeField] private GameEvent pcInteractionEvent;
    [SerializeField] private GameEvent dropDebrisEvent;
    [SerializeField] private GameEvent loadNewScene;
    
    private GMOEventType _gmoEventType = GMOEventType.Empty;

    private PowerUpRoutine _currentRoutine; 

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
                if(_currentRoutine != null)
                    {
                        _currentRoutine.PowerUpRoutineStarted();
                    }

                break;
            }
            case GMOEventType.PCInteract:
            {
                pcInteractionEvent.TriggerEvent();
                break;
            }
            case GMOEventType.DropDebris:
            {
                dropDebrisEvent.TriggerEvent();
                break;
            }
            case GMOEventType.LoadNewScene:
            {
                loadNewScene.TriggerEvent();
                break;
            }
            default: throw new NotImplementedException();
        }
    }

    public void SetPowerUpRoutine(PowerUpRoutine pur)
    {
        _currentRoutine = pur;
    }
}
