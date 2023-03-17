using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePCInteraction : MonoBehaviour
{
    [SerializeField] private GameObject pcInteractionUI;
    [SerializeField] private float delayTime = 5f;

    private bool _isInRoutine;

    private void Awake()
    {
        _isInRoutine = false;
    }

    public void BeginPCInteractionRoutine()
    {
        if (!_isInRoutine)
        {
            _isInRoutine = true;
            StartCoroutine(PCInteractionRoutine());
        }
    }

    private IEnumerator PCInteractionRoutine()
    {
        pcInteractionUI.SetActive(true);
        yield return new WaitForSeconds(delayTime);
        pcInteractionUI.SetActive(false);
        _isInRoutine = false;
    }
}
