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

    public void HandleStartAcid()
    {
        StartCoroutine(StartAcid());
    }
    
    private IEnumerator StartAcid()
    {
        yield return new WaitForSeconds(1.5f);
        _acidSequence = DOTween.Sequence();
        _acidSequence.Append(transform.DOScaleY(20, 10f).SetEase(Ease.OutSine));
        _acidSequence.Append(transform.DOScaleY(175f, 60f));
    }

    public void DrainAcid()
    {
        _acidSequence.Kill();
        transform.DOScaleY(3f, 10f).OnComplete(() => { gameObject.SetActive(false); });
    }
}