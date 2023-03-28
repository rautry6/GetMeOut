using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using static UnityEngine.Debug;

public class AcidManager : MonoBehaviour
{
    private Sequence _acidSequence;
    private Vector3 _startScale;
    private TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> _startTween;

    private void Awake()
    {
        _startScale = transform.localScale;
    }

    public void HandleStartAcid(float delay = 1.5f)
    {
        StartCoroutine(StartAcid(delay));
    }

    private IEnumerator StartAcid(float delay)
    {
        yield return new WaitForSeconds(delay);
        _startTween = transform.DOScaleY(175f, 60f);
    }

    public void DrainAcid()
    {
        transform.DOScaleY(_startScale.y, 10f).OnComplete(() =>
        {
            /*gameObject.SetActive(false);*/
            _startTween.Kill();
        });
    }

    public void ResetAcidScaleToStart()
    {
        StopAllCoroutines();
        _startTween.Complete();
        transform.DOScaleY(_startScale.y, 0f);
        HandleStartAcid(2.25f);
    }
}