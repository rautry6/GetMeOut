using GetMeOut;
using UnityEngine;

public class PowerUpChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PowerUp"))
        {
            PowerUpInjector powerUpInjector = collision.GetComponent<PowerUpInjector>();
            AddPowerUp(powerUpInjector.PowerUp);
            //AutoSave.Instance.Powerups.Add(powerUpInjector.PowerUp);
        }
    }

    public void LoadPowerUps()
    {
        foreach (var powerUp in PowerUpManager.Instance.PowerUpList)
        {
            AddPowerUp(powerUp);
        }
    }
    
    public void AddPowerUp(string powerUp)
    {
        
        if(powerUp == "WallInteractor")
        {
            GetComponent<WallInteractor>().HasWallInteractor = true;
        }
        else if(powerUp == "DoubleJump")
        {
            GetComponent<Jump>().MaxAirJumps = 1;
        }
        else if(powerUp == "Grapple")
        {
            GetComponent<Grapple>().enabled = true;
        }
        
    }
}
