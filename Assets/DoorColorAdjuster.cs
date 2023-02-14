using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DoorColorAdjuster : MonoBehaviour
{
    [SerializeField] private Color colorVariationOne;
    [SerializeField] private Color colorVariationTwo;
    [SerializeField] private Color colorVariationThree;
    [SerializeField] private float seconds;
    private Light2D _light2D;
    private bool _routineIsRunning = false;

    private void Start()
    {
        _light2D = GetComponent<Light2D>();
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
        Debug.Log("hello");
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
}
