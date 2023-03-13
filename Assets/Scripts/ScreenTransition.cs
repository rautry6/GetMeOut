using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private float duration;
    [SerializeField] private Move playerMove;
    [SerializeField] private PlayerHealth playerHealth;

    private readonly List<Image.FillMethod> _fillMethods = new();
    [CanBeNull] private static DoorManager _currentDoorManager;

    private void Awake()
    {
        _fillMethods.Add(Image.FillMethod.Horizontal);
        _fillMethods.Add(Image.FillMethod.Vertical);
        _fillMethods.Add(Image.FillMethod.Radial90);
        _fillMethods.Add(Image.FillMethod.Radial180);
        _fillMethods.Add(Image.FillMethod.Radial360);
    }

    public void HandleStartBackgroundTransition()
    {
        StartCoroutine(BackgroundTransition());
    }

    private IEnumerator BackgroundTransition()
    {
        playerMove.StopMovement();
        playerHealth.UpdateInvulnerable(true);
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

        if (_currentDoorManager != null)
        {
            playerMove.gameObject.transform.position = _currentDoorManager.TransitionTo.position;
            _currentDoorManager.ResetAnimator();
        }

        yield return new WaitForSeconds(.5f);
        elapsedTime = 0;
        while (background.fillAmount > 0)
        {
            background.fillAmount = Mathf.Lerp(background.fillAmount, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            if (background.fillAmount < .1f)
            {
                playerMove.RegainMovement();
                playerHealth.UpdateInvulnerable(false);
            }
            yield return null;
        }

    }

    private bool CoinFlip()
    {
        return Random.Range(0, 2) > 0;
    }

    Image.FillMethod GetRandomFillMethod()
    {
        return _fillMethods[Random.Range(0, _fillMethods.Count)];
    }

    public static void UpdateCurrentDoorManager(DoorManager doorManager)
    {
        _currentDoorManager = doorManager;
    }
}