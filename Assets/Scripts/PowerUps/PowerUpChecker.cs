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
            AutoSave.Instance.Powerups.Add(powerUpInjector.PowerUp);
        }
    }

    public void LoadPowerUps()
    {
        var temp = AutoSave.Instance.Powerups;
        foreach (var powerUp in temp)
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
        
    }
}
