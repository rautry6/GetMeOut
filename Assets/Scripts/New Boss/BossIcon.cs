using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BlindBoss))]
public class BossIcon : MonoBehaviour
{
    [SerializeField] private GameObject bossIcon;
    private float leftBound = -888f;
    private float rightBound = 888f;

    private BlindBoss _bossScript;
    private float totalBossDistanceTraveling;
    private float totalScreenDist;

    private float screenToDistanceRatio;
    private float lastBossXPosition;

    // Start is called before the first frame update
    void Start()
    {
        _bossScript = GetComponent<BlindBoss>();

        totalScreenDist = leftBound - rightBound;
        totalBossDistanceTraveling = _bossScript.totalDistanceToTravel;

        screenToDistanceRatio = Mathf.Abs(totalScreenDist / totalBossDistanceTraveling);
        Debug.Log(screenToDistanceRatio);
    }

    // Update is called once per frame
    void Update()
    {
        bossIcon.transform.localPosition = new Vector3(transform.position.x * screenToDistanceRatio, bossIcon.transform.localPosition.y, bossIcon.transform.localPosition.z);
    } 
}
