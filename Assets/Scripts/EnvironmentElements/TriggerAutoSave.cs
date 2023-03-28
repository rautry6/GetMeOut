using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAutoSave : MonoBehaviour
{
    [SerializeField] private GameEvent autoSaveEvent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            autoSaveEvent.TriggerEvent();
        }
    }
}
