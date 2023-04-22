using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ComingSoonUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        StartCoroutine(FadeOutUI());
    }

    private IEnumerator FadeOutUI()
    {
        yield return new WaitForSeconds(3f);
        canvasGroup.DOFade(0, 1.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnDisable()
    {
        canvasGroup.alpha = 1f;
    }
}
