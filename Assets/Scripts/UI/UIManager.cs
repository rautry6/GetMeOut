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
    [SerializeField] private GameObject restartButton;
    [SerializeField] private float gameOverFadeInTime = 2f;
    [SerializeField] private float gameOverTextFadeInTime = 2f;
    [SerializeField] private GameObject bossUI;

    /// <summary>
    /// Turns off the player health and keycard inventory ui
    /// </summary>
    public void DisableHealthAndKeycardUI()
    {
        healthUI.SetActive(false);
        keycardUI.SetActive(false);
    }
    
    public void EnableHealthAndKeycardUI()
    {
        healthUI.SetActive(true);
        keycardUI.SetActive(true);
    }

    /// <summary>
    /// Turns off the tip ui
    /// </summary>
    public void DisableTipUI()
    {
        tipUI.SetActive(false);
    }
    
    public void EnableTipUI()
    {
        tipUI.SetActive(true);
    }

    public void GameOver()
    {
        DisableHealthAndKeycardUI();
        DisableTipUI();

        Debug.Log("Fading In");
        //Fades in the GameOver UI
        gameOverCanvasGroup.DOFade(1, gameOverFadeInTime).SetDelay(1f).OnComplete(() =>
        {
            Debug.Log("Complete");
            restartButton.SetActive(true);
        });
        gameOverTextCanvasGroup.DOFade(1, gameOverTextFadeInTime);
    }

    public void BossGameOver()
    {
        DisableHealthAndKeycardUI();
        bossUI.SetActive(false);
        gameOverCanvasGroup.DOFade(1, gameOverFadeInTime).SetDelay(1f).OnComplete(() =>
        {
            Debug.Log("Complete");
            restartButton.SetActive(true);
        });
        gameOverTextCanvasGroup.DOFade(1, gameOverTextFadeInTime);
    }
    public void ResetGameOverUI()
    {
        EnableHealthAndKeycardUI();
        EnableTipUI();
        restartButton.SetActive(false);
        //Fades in the GameOver UI
        Debug.Log("Fading Out");
        gameOverCanvasGroup.DOFade(0, gameOverFadeInTime).OnComplete(() =>
        {
            Debug.Log("Complete");
        });
        gameOverTextCanvasGroup.DOFade(0, gameOverTextFadeInTime);
    }

}
