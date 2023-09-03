using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInjector : MonoBehaviour
{
    [Header("PowerUp")]
    public PowerUpTypes Power;

    private string powerUp;

    public enum PowerUpTypes
    {
        WallInteractor,
        DoubleJump,
        Grapple,
        Dash
    }

    public string PowerUp { get { return powerUp; } }

    private void Awake()
    {
        powerUp = Power.ToString();
    }
}