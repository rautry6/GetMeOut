using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HearingManager;
using DG.Tweening;

public class DeafBoss : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Ease easeType;
    [SerializeField] private GameObject rightPoint;
    [SerializeField] private GameObject leftPoint;
    [SerializeField] private float wanderSpeed;
    [SerializeField] private MoveDirection wanderDirection;
    private bool hasArrived = false;

    private bool wandering = true;


    private enum MoveDirection
    {
        Right,
        Left,
    }

    [Header("Hearing")]
    private Vector3 lastHeardSoundLocation;
    private float hearingAccuracy;
    private float distanceFromSource;
    [SerializeField] private float hearingRange = 20f;

    [SerializeField] private float listeningTime = 5f;
    private float listenFor;
    private bool listening = false;

    public float HearingRange { get { return hearingRange; } }

    [Header("Charging")]
    [SerializeField] private float chargeCooldown = 5f;
    private bool charging = false;

    private float health = 100f;

    // Start is called before the first frame update
    void Awake()
    {
        wanderSpeed = (Vector3.Distance(rightPoint.transform.position, leftPoint.transform.position) / moveSpeed) * 2;
        wanderDirection = MoveDirection.Right;
    }

    void Start()
    {
        StartWandering();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasArrived)
        {
            hasArrived = false;
            StartWandering();
        }

        if(listenFor > 0)
        {
            listenFor -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {

    }

    public void ReportSoundHeard(Vector3 location, EHeardSoundCategory category, float intensity)
    {
        //Calculate intesnity based on distance from source
        float newIntensity = intensity / Vector3.Distance(location, transform.position);

        Debug.Log("Heard sound " + category + " at " + location.ToString() + " with intensity of " + newIntensity);

        if(newIntensity < 0.9)
        {
            MoveTowardsLastSound(location);
        }
        else
        {
            Debug.Log("Attack!");
        }
    }

    public void MoveTowardsLastSound(Vector3 location)
    {
        lastHeardSoundLocation = location;
        DOTween.Clear();
        transform.DOMoveX(location.x, moveSpeed).SetEase(easeType).OnComplete(() =>
        {
            StartCoroutine(Listen());
        });
        
    }

    public void Attack()
    {

    }

    public void Wander()
    {

    }


    public IEnumerator Listen()
    {
        listenFor = listeningTime;

        yield return new WaitUntil(() => listenFor <= 0);

        StartWandering();
    }

    public void StartWandering()
    {
 
        if (wanderDirection == MoveDirection.Right)
        {
            transform.DOMoveX(rightPoint.transform.position.x, wanderSpeed).SetEase(easeType).OnComplete(() =>
            {
                wanderDirection = MoveDirection.Left;
                hasArrived = true;
            });
        }
        else
        {
            transform.DOMoveX(leftPoint.transform.position.x, wanderSpeed).SetEase(easeType).OnComplete(() =>
            {
                wanderDirection = MoveDirection.Right;
                hasArrived = true;
            });
        }
    }
}
