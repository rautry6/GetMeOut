using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInteractiveDetection : MonoBehaviour
{
    [SerializeField] private GameObject interactiveDetection;

    public void HandleEnableInteractiveDetection()
    {
        interactiveDetection.SetActive(true);
    }
}
