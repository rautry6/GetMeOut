using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EmergencyLights : MonoBehaviour
{
    //Intensity : 3
    /// <summary>
    /// Radius Inner : -5 Outer : 8
    /// Inner / Outer Spot Angle : 360
    /// Falloff : 0.15
    /// Target Background layer
    /// </summary>
    [SerializeField] Light2D light2D;

    [SerializeField] private bool shrinking = true;

    [Header("Inner Radius")]
    [SerializeField] private float MinimumInnerRadius = -60;
    [SerializeField] private float MaximumInnerRadius = -5;

    [Header("Outer Radius")]
    [SerializeField] private float MinimumOuterRadius = 1;
    [SerializeField] private float MaximumOuterRadius = 8;

    [Header("Flash")]
    [SerializeField] private float flashSpeed = 5f;

    // Start is called before the first frame update
    void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shrinking)
        {
            if (light2D.pointLightInnerRadius > MinimumInnerRadius && light2D.pointLightOuterRadius > MinimumOuterRadius)
            {
                light2D.pointLightInnerRadius -= flashSpeed * Time.deltaTime;
                light2D.pointLightOuterRadius -= flashSpeed * Time.deltaTime;
            }
            else
            {
                shrinking = !shrinking;
            }
        }
        else
        {
            if (light2D.pointLightInnerRadius < MaximumInnerRadius && light2D.pointLightOuterRadius < MaximumOuterRadius)
            {
                light2D.pointLightInnerRadius += flashSpeed * Time.deltaTime;
                light2D.pointLightOuterRadius += flashSpeed * Time.deltaTime;
            }
            else
            {
                shrinking = !shrinking;
            }
        }
    }
}
