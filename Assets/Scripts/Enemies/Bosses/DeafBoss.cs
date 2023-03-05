using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HearingManager;
using DG.Tweening;

public class DeafBoss : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 1.5f;
    [SerializeField] private Ease easeType;
    [SerializeField] private GameObject rightPoint;
    [SerializeField] private GameObject leftPoint;
    [SerializeField] private float wanderSpeed;
    [SerializeField] private MoveDirection wanderDirection;
    private bool hasArrived = false;
    private bool targetContinouslyRunning;

    private Rigidbody2D rigidBody;

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
    private int numberOfSoundsInSuccession = 0;
    private float removeTime = 1.5f;

    [SerializeField] private float listeningTime = 5f;
    private float listenFor;
    private bool listening = false;

    public float HearingRange { get { return hearingRange; } }

    [Header("Charging")]
    [SerializeField] private float chargeCooldown = 5f;
    [SerializeField, Tooltip("How far from the player the AI needs to be to charge")] private float chargeRange = 2f;
    private bool charging = false;

    private float health = 100f;

    private GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        wanderSpeed = (Vector3.Distance(rightPoint.transform.position, leftPoint.transform.position) / moveSpeed) * 2;
        wanderDirection = MoveDirection.Right;
        player = GameObject.Find("Player");
        rigidBody = GetComponent<Rigidbody2D>();
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

        if(numberOfSoundsInSuccession > 0)
        {
            removeTime -= Time.deltaTime;

            if(removeTime <= 0)
            {
                numberOfSoundsInSuccession--;

                if(numberOfSoundsInSuccession > 0)
                {
                    removeTime = Config.Instance.TimeToRemoveSounds;
                }
            }
        }
    }

    private void FixedUpdate()
    {

    }

    public void ReportSoundHeard(Vector3 location, EHeardSoundCategory category, float intensity)
    {
        //Calculate intesnity based on distance from source
        float newIntensity = intensity / Vector3.Distance(location, transform.position);
        numberOfSoundsInSuccession++;
        removeTime = Config.Instance.TimeToRemoveSounds;

        //Debug.Log("Heard sound " + category + " at " + location.ToString() + " with intensity of " + newIntensity);

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
        if(charging)
        {
            return;
        }

        Debug.Log("moe");

        lastHeardSoundLocation = location;
        DOTween.Clear();

        float currentSpeed = 0;

        if(numberOfSoundsInSuccession <= 1) {

            currentSpeed = moveSpeed;
        }
        else if(numberOfSoundsInSuccession <= 3)
        {
            currentSpeed = moveSpeed / 3;
        }
        else
        {
            currentSpeed = sprintSpeed;
            targetContinouslyRunning = true;
        }

        if (Vector3.Distance(location, transform.position) < chargeRange && numberOfSoundsInSuccession > 2)
        {
            if (location.x > transform.position.x)
            {
                Charge(transform.right);
            }
            else
            {
                Charge(-transform.right);
            }
        }
        else
        {

            transform.DOMoveX(location.x, currentSpeed).SetEase(easeType).OnComplete(() =>
            {
                StartCoroutine(Listen());
            });
        }
        
    }

    public void Attack()
    {

    }

    public void Charge(Vector3 direction)
    {
        charging = true;
        DOTween.Clear();

        rigidBody.AddForce(direction * 20, ForceMode2D.Impulse);
        StartCoroutine(CoolDown());
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

    public IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(chargeCooldown);

        charging = false;
        numberOfSoundsInSuccession = 0;
    }
}
