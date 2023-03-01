using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private GameObject healthUI;

    [Header("Keycard UI")]
    [SerializeField] private GameObject keycardUI;

    [Header("Tip UI")]
    [SerializeField] private GameObject tipUI;

    [Header("Game Over UI")]
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private CanvasGroup gameOverTextCanvasGroup;
    [SerializeField] private float gameOverFadeInTime = 2f;
    [SerializeField] private float gameOverTextFadeInTime = 2f;

    /// <summary>
    /// Turns off the player health and keycard inventory ui
    /// </summary>
    public void DisableHealthAndKeycardUI()
    {
        healthUI.SetActive(false);
        keycardUI.SetActive(false);
    }

    /// <summary>
    /// Turns off the tip ui
    /// </summary>
    public void DisableTipUI()
    {
        tipUI.SetActive(false);
    }

    public void GameOver()
    {
        DisableHealthAndKeycardUI();
        DisableTipUI();

        //Fades in the GameOver UI
        gameOverCanvasGroup.DOFade(1, gameOverFadeInTime);
        gameOverTextCanvasGroup.DOFade(1, gameOverTextFadeInTime);
    }
}
