using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    [SerializeField] private GameObject destinationVent;

    public GameObject DestinationVent => destinationVent;
}
