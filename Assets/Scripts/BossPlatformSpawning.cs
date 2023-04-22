using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatformSpawning : MonoBehaviour
{
    [SerializeField] private float timeBetweenPlatforms;
    private bool platformsActive = false;
    private float spawnTimer;
    [SerializeField] private GameObject[] platformingSegments;
    private int currentSegment = 0;
    [SerializeField] private GameEvent SpawnNextDebris;

    [SerializeField]private bool canSpawn = false;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = timeBetweenPlatforms;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawn)
        {
            return;
        }

        if (!platformsActive)
        {
            spawnTimer -= Time.deltaTime;

            if(spawnTimer < 0)
            {
                platformsActive= true;
                if (currentSegment < platformingSegments.Length)
                {
                    SpawnPlatforms();
                }
            }
        }
    }

    public void SpawnPlatforms()
    {
        platformingSegments[currentSegment].SetActive(true);
        SpawnNextDebris.TriggerEvent();
    }

    public void ButtonPressed()
    {
        platformingSegments[currentSegment].SetActive(false);
        currentSegment++;
        spawnTimer = timeBetweenPlatforms;
        platformsActive = false;
    }

    public void EnableSpawning()
    {
        canSpawn = true;
    }
}
