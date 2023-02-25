using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.Debug;
public class AcidManager : MonoBehaviour
{
    [SerializeField] private Transform endPositions;
    [SerializeField] private float initialMoveDuration;
    [SerializeField] private float secondaryMoveDuration;
    [SerializeField] private float startDelay;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject horizontalPlatform;
    [SerializeField] private Ease acidEase;
    private Sequence _acidSequence;
    private Transform _startingPosition;
    private bool _runOnce = false;

    private void Awake()
    {
        _acidSequence.SetId("acid");
    }

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform;
    }

    private void Update()
    {
        var player = Physics2D.CircleCast(horizontalPlatform.transform.position, 1f, Vector2.up, 1f, playerLayer);
        
        if (player.transform != null)
        {
            if (!_runOnce)
            {
                _runOnce = true;
                StartCoroutine(StartAcid());
            }
        }
    }

    private IEnumerator StartAcid()
    {
        yield return new WaitForSeconds(1f);
        cameraShake.ShakeCamera();
        yield return new WaitForSeconds(1f);
        _acidSequence = DOTween.Sequence();
        _acidSequence.Append(transform.DOMoveY(endPositions.position.y, initialMoveDuration).SetEase(acidEase));
    }

    public void DrainAcid()
    {
        _acidSequence.Kill();
        transform.DOMoveY(-20, 10f).OnComplete(() => { gameObject.SetActive(false); });
    }
}