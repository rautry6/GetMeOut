using GetMeOut;
using UnityEngine;

public class PowerUpChecker : MonoBehaviour
{
    [SerializeField] private GameObject dbljump_icon;
    [SerializeField] private GameObject walljump_icon;
    [SerializeField] private GameObject dash_icon;
    [SerializeField] private GameObject grapple_icon;

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
        var powerUpHolder = GameObject.Find("Powerup Icons");
        dbljump_icon = powerUpHolder.transform.GetChild(0).gameObject;
        walljump_icon = powerUpHolder.transform.GetChild(1).gameObject;
        grapple_icon = powerUpHolder.transform.GetChild(2).gameObject;
        dash_icon = powerUpHolder.transform.GetChild(3).gameObject;
        if(powerUp == "WallInteractor")
        {
            walljump_icon.SetActive(true);
            GetComponent<WallInteractor>().HasWallInteractor = true;
        }
        else if(powerUp == "DoubleJump")
        {
            dbljump_icon.SetActive(true);
            GetComponent<Jump>().MaxAirJumps = 1;
        }
        else if(powerUp == "Grapple")
        {
            grapple_icon.SetActive(true);
            GetComponent<Grapple>().enabled = true;
        }
        else if(powerUp == "Dash")
        {
            dash_icon.SetActive(true);
            GetComponent<Dash>().enabled = true;
        }
        
    }
}
