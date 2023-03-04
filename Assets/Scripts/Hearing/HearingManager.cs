using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingManager : MonoBehaviour
{
    public static HearingManager Instance;

    public List<HearingSensor> AllSensors = new List<HearingSensor>();

    //List of sound types
    public enum EHeardSoundCategory
    {
        EFootstep,
        EJump
    }

    private void Awake()
    {
        //Destroys the extra HearingManagers if any are found
        if (Instance != null)
        {
            Debug.LogError("Multiple HearingMangers found. Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Register(HearingSensor sensor)
    {
        AllSensors.Add(sensor);
    }

    public void DeRegister(HearingSensor sensor)
    {
        AllSensors.Remove(sensor);
    }

    public void OnSoundEmitted(Vector3 location, EHeardSoundCategory category, float intensity)
    {
        //notifay all sensors
        foreach(var sensor in AllSensors)
        {
            sensor.OnSoundHeard(location, category, intensity); 
        }
    }
}
