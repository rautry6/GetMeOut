using GetMeOut;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PowerUp")
        {
            Debug.Log("PowerUP!");

            PowerUpInjector powerUpInjector = collision.GetComponent<PowerUpInjector>();

            if(powerUpInjector.PowerUp == "WallInteractor")
            {
                GetComponent<WallInteractor>().HasWallInteractor = true;
            }
            else if(powerUpInjector.PowerUp == "DoubleJump")
            {
                GetComponent<Jump>().MaxAirJumps = 1;
            }
        }
    }
}
