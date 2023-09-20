using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTrapAnimation : MonoBehaviour
{
    [SerializeField]Texture[] frames;
    int framesPerSecond = 10;

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] ElectricityTrap eTrap;

    float time = 0;

    void Update()
    {
        if (eTrap.On)
        {
            lineRenderer.widthMultiplier = 10;

            framesPerSecond = (int)(frames.Length / eTrap.OffSpeed);

            int index = (int)((time * framesPerSecond) % frames.Length);

            lineRenderer.material.mainTexture = frames[index];

            time += Time.deltaTime;
        }
        else
        {
            time = 0;
        }
    }
}
