using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using static UnityEngine.Debug;

public class AcidManager : MonoBehaviour
{
    [SerializeField] private float startTime;
    [SerializeField] private Transform finishedPosition;
    private Sequence _acidSequence;
    private Vector3 _startPosition;
    private TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> _startTween;

    public AcidState CurrentAcidState { get; set; }

    private void Awake()
    {
        _startPosition = transform.position;
        CurrentAcidState = AcidState.HasNotStarted;
    }

    public void HandleStartAcid(float delay = 1.5f)
    {
        StartCoroutine(StartAcid(delay));
    }

    private IEnumerator StartAcid(float delay)
    {
        CurrentAcidState = AcidState.ShouldRestart;
        yield return new WaitForSeconds(delay);
        _startTween = transform.DOMoveY(finishedPosition.position.y, startTime);
    }

    public void DrainAcid()
    {
        CurrentAcidState = AcidState.IsDrained;
        transform.DOMoveY(_startPosition.y, 10f).OnComplete(() =>
        {
            /*gameObject.SetActive(false);*/
            _startTween.Kill();
        });
    }

    public void ResetAcidScaleToStart()
    {
        if (CurrentAcidState != AcidState.ShouldRestart) return;
        
        StopAllCoroutines();
        _startTween.Complete();
        transform.DOMoveY(_startPosition.y, 0f);
        HandleStartAcid(2.25f);
    }
}