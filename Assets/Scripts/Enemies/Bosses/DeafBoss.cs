using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HearingManager;

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
        Debug.Log("Heard sound " + category + " at " + location.ToString() + " with intensity of " + intensity);
    }
}
