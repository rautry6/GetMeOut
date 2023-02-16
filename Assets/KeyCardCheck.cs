using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class KeyCardCheck : MonoBehaviour
{
    [SerializeField] private KeyCardColors doorColor;
    [SerializeField] private float totalTime;
    [SerializeField] private float targetIntensity;
    [SerializeField] private Light2D _light2D;
    [SerializeField] private DoorColorAdjuster doorColorAdjuster;
    
    private float _elapsedTime = 0f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Move>();
        if (player != null)
        {
            var keyCardManager = player.GetComponentInChildren<KeyCardManager>();
            if (keyCardManager.CheckIfManagerHasCorrectCard(doorColor))
            {
                StartCoroutine(CorrectCardFound());
            }
        }
    }

    IEnumerator CorrectCardFound()
    {
        while (_elapsedTime < totalTime)
        {
            Debug.Log(_elapsedTime);
            _light2D.intensity = Mathf.Lerp(_light2D.intensity, targetIntensity, (_elapsedTime / totalTime));
            _elapsedTime += Time.deltaTime;
            yield return null; // wait for a frame
        }

        _light2D.intensity = targetIntensity;
        doorColorAdjuster.CorrectKeyCardWasScanned();
        yield return null;
    }
}
