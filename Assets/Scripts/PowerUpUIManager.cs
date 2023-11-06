using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpUIManager : MonoBehaviour
{
    [SerializeField] private GameObject doubleJumpUI;
    [SerializeField] private GameObject wallJumpUI;
    [SerializeField] private GameObject dashUI;
    [SerializeField] private GameObject grappleUI;
    
    private void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "DeafBoss")
        {
            foreach (var powerUp in PowerUpManager.Instance.PowerUpList)
            {
                Debug.Log($"PU: {powerUp}");
                if (powerUp == "DoubleJump")
                {
                    doubleJumpUI.SetActive(true);
                }
                if (powerUp == "WallInteractor")
                {
                    wallJumpUI.SetActive(true);
                }
                if (powerUp == "Dash")
                {
                    dashUI.SetActive(true);
                }
                if (powerUp == "Grapple")
                {
                    grappleUI.SetActive(true);
                }
            }        
        }
    }
}
