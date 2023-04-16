using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private Color colorVariationOne;
    [SerializeField] private Color colorVariationTwo;
    [SerializeField] private Color colorVariationThree;
    [SerializeField] private float seconds;
    [SerializeField] private SpriteRenderer animatedDoor;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Transform transitionTo;
    [SerializeField] private GameObject interactionDetection;
    [SerializeField] private Light2D keyScannerLight;
    public Transform TransitionTo => transitionTo;

    private Light2D _light2D;
    private float _initialIntensity;
    private bool _routineIsRunning = false;

    private void Start()
    {
        _light2D = GetComponent<Light2D>();
        _initialIntensity = _light2D.intensity;
    }

    private void Update()
    {
        if (!_routineIsRunning)
        {
            _routineIsRunning = true;
            StartCoroutine(CycleLights());
        }
    }

    // Start is called before the first frame update
    private IEnumerator CycleLights()
    {
        yield return new WaitForSeconds(seconds);
        _light2D.color = colorVariationOne;
        yield return new WaitForSeconds(seconds);
        _light2D.color = colorVariationTwo;
        yield return new WaitForSeconds(seconds);
        _light2D.color = colorVariationThree;
        yield return new WaitForSeconds(seconds);
        _light2D.color = colorVariationTwo;
        yield return new WaitForSeconds(seconds);
        _light2D.color = colorVariationOne;
        _routineIsRunning = false;
    }

    private void OnDisable()
    {
        StopCoroutine(CycleLights());
    }

    public void CorrectKeyCardWasScanned()
    {
        _light2D.color = colorVariationOne;
        _light2D.intensity = 3f;
        animatedDoor.enabled = false;
        _light2D.enabled = false;
        doorAnimator.Play("Door_Open_Retro");
    }
    public void ResetAnimator()
    {
        doorAnimator.SetTrigger("Reset");
        interactionDetection.SetActive(false);
        keyScannerLight.intensity = 0.2f;
        _light2D.enabled = true;
        _light2D.intensity = _initialIntensity;
    }
}