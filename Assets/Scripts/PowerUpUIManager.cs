using System;
using System.Collections;
using System.Collections.Generic;
using GetMeOut;
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
            HandlePowerUpIcons();
        }
    }

    private void HandlePowerUpIcons()
    {
        var player = GameObject.Find("Player");
        var powerUpHolder = GameObject.Find("Powerup Icons");
        doubleJumpUI = powerUpHolder.transform.GetChild(0).gameObject;
        wallJumpUI = powerUpHolder.transform.GetChild(1).gameObject;
        grappleUI = powerUpHolder.transform.GetChild(2).gameObject;
        dashUI = powerUpHolder.transform.GetChild(3).gameObject;
        foreach (var powerUp in PowerUpManager.Instance.PowerUpList)
        {
            Debug.Log($"PU: {powerUp}");
            if (powerUp == "DoubleJump")
            {
                doubleJumpUI.SetActive(true);
                player.GetComponent<Jump>().MaxAirJumps = 1;
            }

            if (powerUp == "WallInteractor")
            {
                wallJumpUI.SetActive(true);
                player.GetComponent<WallInteractor>().HasWallInteractor = true;
            }

            if (powerUp == "Dash")
            {
                dashUI.SetActive(true);
                player.GetComponent<Dash>().enabled = true;
            }

            if (powerUp == "Grapple")
            {
                player.GetComponent<Grapple>().enabled = true;
                grappleUI.SetActive(true);
            }
        }
    }
}
