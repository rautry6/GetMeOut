using GetMeOut;
using UnityEngine;

public class PowerUpChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PowerUp"))
        {
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
