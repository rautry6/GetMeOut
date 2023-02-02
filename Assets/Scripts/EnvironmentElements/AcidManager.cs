using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AcidManager : MonoBehaviour
{
    [SerializeField] private Transform endPositions;
    [SerializeField] private float initialMoveDuration;
    [SerializeField] private float secondaryMoveDuration;
    [SerializeField] private float startDelay;
    [SerializeField] private CameraShake cameraShake;

    private Sequence _acidSequence;
    private Transform _startingPosition;

    private void Awake()
    {

        _acidSequence.SetId("acid");
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform;
        StartCoroutine(StartDelay());
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        cameraShake.ShakeCamera();
        _acidSequence = DOTween.Sequence();
        _acidSequence.Append(transform.DOMoveY(endPositions.position.y, initialMoveDuration).SetEase(Ease.Linear));
    }

    public void DrainAcid()
    {
        _acidSequence.Kill();
        transform.DOMoveY(-20, 7.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
