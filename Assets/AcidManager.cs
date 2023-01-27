using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AcidManager : MonoBehaviour
{
    [SerializeField] private Transform endPositon;
    [SerializeField] private float moveDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveY(endPositon.position.y, moveDuration, false).SetEase(Ease.Linear);
    }

}
