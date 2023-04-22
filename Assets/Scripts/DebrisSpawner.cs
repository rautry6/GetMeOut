using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] debris;
    private int currentDebris;

    private void Start()
    {
        foreach(var debr in debris)
        {
            debr.SetActive(false);
        }
    }

    public void SpawnNextDebris()
    {

        if (currentDebris < debris.Length)
        {
            debris[currentDebris].SetActive(true);
            currentDebris++;
        }
    }
}
