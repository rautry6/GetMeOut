using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HearingManager;
using DG.Tweening;

public class DeafBoss : MonoBehaviour
{
    [Header("Hearing")]
    private Vector3 lastHeardSoundLocation;
    private float hearingAccuracy;
    private float distanceFromSource;
    [SerializeField] private float hearingRange = 20f;
    public float HearingRange { get { return hearingRange; } }

    [Header("Charging")]
    [SerializeField] private float chargeCooldown = 5f;
    private bool charging = false;

    private float health = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReportSoundHeard(Vector3 location, EHeardSoundCategory category, float intensity)
    {
        //Calculate intesnity based on distance from source
        float newIntensity = 1 / Vector3.Distance(location, transform.position);
        lastHeardSoundLocation = location;

        Debug.Log("Heard sound " + category + " at " + location.ToString() + " with intensity of " + newIntensity);

        if(newIntensity < 0.9)
        {

        }
        else
        {
            Debug.Log("Attack!");
        }
    }
}
