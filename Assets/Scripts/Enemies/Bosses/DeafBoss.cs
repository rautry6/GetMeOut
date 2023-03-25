using System.Collections;
using UnityEngine;
using static HearingManager;
using DG.Tweening;


public class DeafBoss : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private Ease easeType;
    [SerializeField] private GameObject rightPoint;
    [SerializeField] private GameObject leftPoint;
    [SerializeField] private float wanderSpeed;
    [SerializeField] private MoveDirection wanderDirection;
    private bool hasArrived = false;
    private bool targetContinouslyRunning;
    private bool moveToPoint = false;

    private float currentSpeed = 0;
    private GameObject targetPoint;

    private Rigidbody2D rigidBody;

    private enum MoveDirection
    {
        Right,
        Left,
    }

    private enum States
    {
        Listen,
        Investigate,
        Chase,
        Charge,
        Wander,
        Cooldown,
        ChargeDebris,
    }

    [Header("Current State")]
    [SerializeField] private States currentState = States.Wander;
    private bool chasing = false;

    [Header("Hearing")]
    private Vector3 lastHeardSoundLocation;
    private float hearingAccuracy;
    private float distanceFromSource;
    [SerializeField] private float hearingRange = 20f;
    private int numberOfSoundsInSuccession = 0;
    private float removeTime = 1.5f;

    private Vector3 targetLocation; //Used for chasing
    [SerializeField] private float listeningTime = 5f;
    private float listenFor;
    private bool listening = false;

    public float HearingRange { get { return hearingRange; } }

    [Header("Charging")]
    [SerializeField] private float chargeCooldown = 5f;
    [SerializeField, Tooltip("How far from the player the AI needs to be to charge")] private float chargeRange = 2f;
    [SerializeField] private float chargeDistance = 10f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float wallDetectionRange = 0.5f;
    [SerializeField] private LayerMask hitLayer;
    private Vector3 chargeDirection;
    private float chargeEndXPoint;
    private bool charging = false;

    private float health = 100f;

    private GameObject player;
    private Vector3 target;

    [Header("UI")]
    [SerializeField] private GameObject BossHealthBar;

    // Start is called before the first frame update
    void Awake()
    {
        targetPoint = rightPoint;
        wanderSpeed = (Vector3.Distance(targetPoint.transform.position, transform.position) / moveSpeed) * 2;
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
        //Checks if the boss hits a wall while charging
       RaycastHit2D hit = Physics2D.Raycast(transform.position, chargeDirection, wallDetectionRange, hitLayer);
       Debug.DrawRay(transform.position, chargeDirection * wallDetectionRange, Color.yellow);
        if (hit.collider != null && currentState == States.Charge || hit.collider != null && currentState == States.ChargeDebris)
        {
            Debug.Log("fire");
            currentState = States.Cooldown;
            StartCoroutine(CoolDown());
        }


        if (hasArrived && currentState == States.Wander)
        {
            hasArrived = false;
            StartWandering();
        }

        if(listenFor > 0)
        {
            listenFor -= Time.deltaTime;
        }

        //Removes heard sounds from the list to only trigger chasing if they heard in quick succession
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

        if (currentState == States.Investigate)
        {
            var heading = transform.position - lastHeardSoundLocation;
            var distance = heading.magnitude;
            var direction = heading / distance;

            transform.position = new Vector3(transform.position.x - direction.x * currentSpeed * Time.deltaTime, transform.position.y, transform.position.z);

            if(transform.position.x == lastHeardSoundLocation.x)
            {
                StartCoroutine(Listen());
                currentState = States.Listen;
            }
        }
    }

    private void FixedUpdate()
    {
        if(currentState == States.Chase)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < chargeRange)
            {

                if (player.transform.position.x < transform.position.x)
                {
                    chargeDirection = -transform.right;

                }
                else
                {
                    chargeDirection = transform.right;
                }

                chargeEndXPoint = transform.position.x + (chargeDistance * chargeDirection.x);

                currentState = States.Charge;
                return;
            }

            var heading = transform.position - player.transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance;

            transform.position -= transform.right * direction.x * sprintSpeed * Time.deltaTime;

        }

        if(currentState == States.Charge)
        {
            if(chargeDirection.x >= 0)
            {
                if(transform.position.x  < chargeEndXPoint)
                {
                    transform.position += transform.right * chargeDirection.x * chargeSpeed * Time.deltaTime;
                }
                else
                {
                    currentState = States.Cooldown;
                    StartCoroutine(CoolDown());
                }
            }
            else
            {
                if (transform.position.x > chargeEndXPoint)
                {
                    transform.position += transform.right * chargeDirection.x * chargeSpeed * Time.deltaTime;
                }
                else
                {
                    currentState = States.Cooldown;
                    StartCoroutine(CoolDown());
                }
            }
        }

        if(currentState == States.ChargeDebris)
        {
            if(chargeDirection.x >= 0)
            {
                if (transform.position.x < chargeEndXPoint)
                {
                    transform.position += transform.right * chargeDirection.x * chargeSpeed * Time.deltaTime;
                }
                else
                {
                    currentState = States.Cooldown;
                    StartCoroutine(CoolDown());
                }
            }
            else
            {
                if (transform.position.x > chargeEndXPoint)
                {
                    transform.position += transform.right * chargeDirection.x * chargeSpeed * Time.deltaTime;
                }
                else
                {
                    currentState = States.Cooldown;
                    StartCoroutine(CoolDown());
                }
            }
        }
    }

    public void ReportSoundHeard(Vector3 location, EHeardSoundCategory category, float intensity)
    {
        //Calculate intesnity based on distance from source
        float newIntensity = intensity / Vector3.Distance(location, transform.position);
        numberOfSoundsInSuccession++;
        removeTime = Config.Instance.TimeToRemoveSounds;

        //Debug.Log("Heard sound " + category + " at " + location.ToString() + " with intensity of " + newIntensity);

        if (category == EHeardSoundCategory.ECrash)
        {
            target = location;

            if (target.x < transform.position.x)
            {
                chargeDirection = -transform.right;

            }
            else
            {
                chargeDirection = transform.right;
            }

            chargeEndXPoint = target.x;

            currentState = States.ChargeDebris;
            DOTween.Clear();
        }
        else if (newIntensity < 0.9 && currentState != States.Chase)
        {
            MoveTowardsLastSound(location);
        }
        else
        {
            //Debug.Log("Attack!");
        }
    }

    public void MoveTowardsLastSound(Vector3 location)
    {
        if(currentState == States.Charge || currentState == States.Chase || currentState == States.Cooldown)
        {
            return;
        }
        else
        {
            currentState = States.Investigate;
        }


        lastHeardSoundLocation = location;
        DOTween.Clear();

        if(numberOfSoundsInSuccession <= 1) {

            currentSpeed = moveSpeed;
        }
        else if(numberOfSoundsInSuccession <= 2)
        {
            currentSpeed = moveSpeed * 1.5f;
        }
        else
        {
            //Start chasing player
            targetLocation = location;
            currentState = States.Chase;
            DOTween.Clear();
            return;
        }
        
    }

    public void Chase()
    {

    }

    public void Attack()
    {

    }


    public IEnumerator Listen()
    {
        listenFor = listeningTime;

        yield return new WaitUntil(() => listenFor <= 0);

        //Has not heard anything so start wandering
        if (currentState == States.Listen)
        {
            currentState = States.Wander;
            StartWandering();
        }
    }

    public void StartWandering()
    {

        if (currentState == States.Investigate)
        {
            return;
        }


        if (wanderDirection == MoveDirection.Right)
        {
            targetPoint = rightPoint;
            wanderSpeed = (Vector3.Distance(targetPoint.transform.position, transform.position) / moveSpeed) * 2;

            transform.DOMoveX(rightPoint.transform.position.x, wanderSpeed).SetEase(easeType).OnComplete(() =>
            {
                wanderDirection = MoveDirection.Left;
                hasArrived = true;
            });
        }
        else
        {
            targetPoint = leftPoint;
            wanderSpeed = (Vector3.Distance(targetPoint.transform.position, transform.position) / moveSpeed) * 2;

            transform.DOMoveX(leftPoint.transform.position.x, wanderSpeed).SetEase(easeType).OnComplete(() =>
            {
                wanderDirection = MoveDirection.Right;
                hasArrived = true;
            });
        }
    }

    public IEnumerator CoolDown()
    {
        yield return null;
        yield return new WaitForSeconds(chargeCooldown);

        currentState = States.Wander;
        StartWandering();
        numberOfSoundsInSuccession = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Debris")
        {
            currentState = States.Cooldown;
            TakeDamage(33.5f);
            StartCoroutine(CoolDown());
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            //Makes sure health is not negative before updating the UI
            health = 0;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        BossHealthBar.transform.localScale = new Vector3(health, BossHealthBar.transform.localScale.y);
    }


    public IEnumerator UITest()
    {
        yield return new WaitForSeconds(2f);

        TakeDamage(33.5f);

        yield return new WaitForSeconds(2f);

        TakeDamage(33.5f);

        yield return new WaitForSeconds(2f);

        TakeDamage(33.5f);
    }
}
