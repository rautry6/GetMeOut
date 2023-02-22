using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private float duration;

    private readonly List<Image.FillMethod> _fillMethods = new();

    private void Awake()
    {
        _fillMethods.Add(Image.FillMethod.Horizontal);
        _fillMethods.Add(Image.FillMethod.Vertical);
        _fillMethods.Add(Image.FillMethod.Radial90);
        _fillMethods.Add(Image.FillMethod.Radial180);
        _fillMethods.Add(Image.FillMethod.Radial360);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(BackgroundTransition());
    }

    public IEnumerator BackgroundTransition()
    {
        var elapsedTime = 0f;
        var fillMethodType = GetRandomFillMethod();
        if (fillMethodType == Image.FillMethod.Horizontal)
        {
            background.fillOrigin =
                Random.Range(0, 10) > 5 ? (int) Image.OriginHorizontal.Left : (int) Image.OriginHorizontal.Right;
        }
        else if (fillMethodType == Image.FillMethod.Vertical)
        {
            background.fillOrigin =
                Random.Range(0, 10) > 5 ? (int) Image.OriginVertical.Top : (int) Image.OriginVertical.Bottom;
        }
        else if (fillMethodType == Image.FillMethod.Radial90)
        {
            var random = Random.Range(0, 10);
            background.fillClockwise = CoinFlip();
            background.fillOrigin = random > 6 ? (int) Image.Origin90.BottomLeft :
                random > 4 ? (int) Image.Origin90.BottomRight :
                random > 2 ? (int) Image.Origin90.TopLeft : (int) Image
                    .Origin90.TopRight;
        }
        else if (fillMethodType == Image.FillMethod.Radial180)
        {
            var random = Random.Range(0, 10);
            background.fillClockwise = CoinFlip();
            background.fillOrigin = random > 6 ? (int) Image.Origin180.Bottom :
                random > 4 ? (int) Image.Origin180.Left :
                random > 2 ? (int) Image.Origin180.Right : (int) Image
                    .Origin180.Top;
        }
        else if (fillMethodType == Image.FillMethod.Radial360)
        {
            var random = Random.Range(0, 10);
            background.fillClockwise = CoinFlip();
            background.fillOrigin = random > 6 ? (int) Image.Origin360.Bottom :
                random > 4 ? (int) Image.Origin360.Left :
                random > 2 ? (int) Image.Origin360.Right : (int) Image
                    .Origin360.Top;
        }

        background.fillMethod = fillMethodType;
        while (background.fillAmount < 1)
        {
            background.fillAmount = Mathf.Lerp(background.fillAmount, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        background.fillAmount = 0;
    }

    private bool CoinFlip()
    {
        return Random.Range(0, 2) > 0;
    }

    Image.FillMethod GetRandomFillMethod()
    {
        return _fillMethods[Random.Range(0, _fillMethods.Count)];
    }
}