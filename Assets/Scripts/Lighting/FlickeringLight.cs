using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] Light2D light2D;

    [SerializeField] private bool flickering = false;

    [Header("Flicker")]
    [SerializeField] private float flickerSpeed = 0.5f;
    [SerializeField] private float numberOfFlickers = 0f;
    [SerializeField] private float timeBetweenFlickers = 0f;
    [SerializeField] private float timer = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        light2D = GetComponent<Light2D>();
        timeBetweenFlickers = Random.Range(2, 6);
        numberOfFlickers = Random.Range(1, 3);
        flickerSpeed = Random.Range(0.2f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < timeBetweenFlickers)
        {
            timer += Time.deltaTime;
        }else if (!flickering)
        {
            flickering = true;
            StartCoroutine(Flicker());
        }
    }

    public IEnumerator Flicker()
    {

        for (int i = 0; i < numberOfFlickers; i++)
        {
            light2D.enabled = false;
            yield return new WaitForSeconds(flickerSpeed);
            light2D.enabled = true;
            yield return new WaitForSeconds(flickerSpeed);
        }

        timer = 0;

        timeBetweenFlickers = Random.Range(0.2f, 1);
        numberOfFlickers = Random.Range(1, 3);
        flickerSpeed = Random.Range(0.2f, 1);

        flickering = false;
    }
}
