using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Grapple : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] private Camera cam;

    [SerializeField] private CinemachineVirtualCamera normalCam;
    [SerializeField] private CinemachineVirtualCamera grappleCam;



    [SerializeField] LayerMask grappleLayer;
    [SerializeField] LayerMask groundLayer;

    private Vector3 targetPosition;

    [SerializeField] private DistanceJoint2D distanceJoint;


    [SerializeField] private bool shooting = false;
    [SerializeField] private bool latched = false;
    [SerializeField] private bool returning = false;
    private bool grappling = false;

    [Header("Grapple Variables")]
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float travelSpeed;
    [SerializeField] private float retractSpeed;
    [SerializeField] private float maxRopeLength;
    [SerializeField] private float minRopeLength;
    [SerializeField] private float pullStrength;

    [SerializeField] private bool finishedShooting = false;
    [SerializeField] private bool shortenRope = false;
    [SerializeField] private float pullSpeed = 0.5f;

    [Header("Rope Variables")]
    [SerializeField] private int numberOfPoints = 40;
    [SerializeField] private bool snap = false;
    [SerializeField] private float ropeChangeAmount = 0.05f;

    private Transform snapPoint;

    [Header("Rope Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    [Range(0.01f, 4)][SerializeField] private float StartWaveSize = 2;
    float waveSize = 0;

    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField][Range(1, 50)] private float ropeProgressionSpeed = 1;

    [SerializeField] private GameObject selectedGrapple;
    private GameObject[] grapples;

    float moveTime = 0;

    private Transform currentGrapple;

    private MovingGrappleHook currentMovingHook;

    private Transform snapPosition;

    // Start is called before the first frame update
    void Start()
    {
        distanceJoint = GetComponent<DistanceJoint2D>();

        lineRenderer.positionCount = numberOfPoints;

        lineRenderer.enabled = false;
        distanceJoint.enabled = false;

        cam = Camera.main;

        grapples = GameObject.FindGameObjectsWithTag("Grapple");
    }

    // Update is called once per frame
    void Update()
    {

        CheckForNearestGrappleHook();

        

    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            targetPosition = cam.ScreenToWorldPoint(Input.mousePosition);




            if (!returning && !latched && !shooting)
            {
                grappling = true;
                shooting = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopGrappling();
        }

        if (!grappling)
        {
            lineRenderer.enabled = false;
            distanceJoint.enabled = false;

            return;
        }

        lineRenderer.enabled = true;

        if (returning)
        {
            distanceJoint.enabled = false;

            return;

        }

        //Gets the direction from the target to the player and normalizes it
        var direction = targetPosition - transform.position;
        direction.Normalize();

        //Gets the length between the player and target position
        var length = Vector2.Distance(transform.position, targetPosition);

        //Makes sure the player is not clicking past the maxTravelDistance mark
        if (length > maxTravelDistance)
        {
            length = maxTravelDistance;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, length, groundLayer);

        if (hit.collider == null)
        {
            //Checks for a Raycast hit on the specified layers
            hit = Physics2D.Raycast(transform.position, direction, length, grappleLayer);
        }

        moveTime += Time.deltaTime;

        if (!snap)
        {
            DrawRope(hit);
        }
        else
        {

            if (snapPosition != null)
            {
                
                SnapRope(snapPosition);
            }

            if (Input.GetKey(KeyCode.W))
            {
                if (distanceJoint.enabled && distanceJoint.distance > minRopeLength)
                {
                    distanceJoint.distance -= ropeChangeAmount * Time.deltaTime;
                }
            }

            if (Input.GetKey(KeyCode.S))
            {
                if (distanceJoint.enabled && distanceJoint.distance < maxRopeLength)
                {
                    distanceJoint.distance += ropeChangeAmount * Time.deltaTime;
                }
            }
        }

        if (shooting)
        {
            shooting = false;

            StartCoroutine(CheckIfHit(hit));
        }
    }

    public void DrawRope(RaycastHit2D hit)
    {
        if (hit.collider != null && (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Wall")))
        {
            targetPosition = hit.point;
        }
        
        for (int i = 0; i < numberOfPoints; i++)
        {
            float delta = (float)i / ((float)numberOfPoints - 1f);
            var distance = targetPosition - transform.position;

            //New Code
            /*Vector2 offset = Vector2.Perpendicular(distance).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;

            var newTargetPosition = transform.position + (distance * maxTravelDistance);

            Vector2 tPosition = Vector2.Lerp(transform.position, newTargetPosition, delta) + offset; */


            Vector2 offset = Vector2.Perpendicular(distance).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            Vector2 tPosition = Vector2.Lerp(transform.position, targetPosition, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(transform.position, tPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            lineRenderer.SetPosition(i, currentPosition);

            //Once the line is finished moving, set finishedShooting to true || if the distance is greater or equal to max travel distance
            if (currentPosition == (Vector2)targetPosition || Vector2.Distance(transform.position, currentPosition) >= maxTravelDistance)
            {
                if (hit == true)
                {
                    //Needs to be equal to the number of the grapple layer
                    if (hit.collider.gameObject.layer == 11)
                    {
                        snapPosition = hit.collider.gameObject.transform;

                        //If the player is hooked to a moving hook, move hook start moving hook
                        if (hit.transform.name.Contains("Moving"))
                        {
                            currentMovingHook = hit.transform.GetComponent<MovingGrappleHook>();

                            if (currentMovingHook.moving != true)
                            {
                                currentMovingHook?.PlayerSnapped();
                            }
                        }

                        SnapRope(snapPosition);
                        snap = true;
                    }
                }

                finishedShooting = true;


            }
        }

    }

    public void SnapRope(Transform hit)
    {
        lineRenderer.positionCount = 2;

        if (grappleCam.Priority < 11)
        {
            grappleCam.Priority = 11;
        }

        if (snapPosition != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.position);
        }

    }

    private void StopGrappling()
    {
        //If the player was hooked to a moving hook, move hook back to starting location
        if (currentMovingHook != null)
        {
            currentMovingHook.PlayerUnSnapped();
            currentMovingHook = null;
        }

        lineRenderer.enabled = false;
        distanceJoint.enabled = false;
        finishedShooting = false;

        grappling = false;
        shooting = false;
        moveTime = 0;
        lineRenderer.positionCount = numberOfPoints;
        snap = false;
        grappleCam.Priority = 8;
    }

    private IEnumerator CheckIfHit(RaycastHit2D hit)
    {
        //Waits until the line is finished being drawn to the target position
        yield return new WaitWhile(() => !finishedShooting);

        //If something was hit
        if(hit != false) {
            Debug.Log(hit.transform.tag);
        }
        if (hit != false && hit.transform.CompareTag("Grapple"))

        {
            Debug.Log(hit.collider);

            //Get the distance to what the rope is snapped to
            var distance = Vector2.Distance(transform.position, hit.collider.transform.position);

            //If the distance is less than the maxRopeLength use the shorter distance 
            if (distance < maxRopeLength)
            {
                distanceJoint.distance = distance;
            }
            else
            {
                distanceJoint.distance = maxRopeLength;
            }

            //Enable spring joint and set connectedBody
            distanceJoint.enabled = true;
            distanceJoint.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
        }
        else
        {
            //Otherwise stop grappling
            StopGrappling();
        }
    }

    public void BreakHook()
    {
        StopGrappling();
    }

    public void CheckForNearestGrappleHook()
    {
        float distance = maxTravelDistance + 0.1f;
        foreach (var grapple in grapples)
        {
            float currentDistance = Vector2.Distance(grapple.transform.position, transform.position);
            if (currentDistance < distance)
            {
                if (currentGrapple != null &&  currentGrapple != grapple.transform)
                {
                    currentGrapple.GetComponent<SpriteRenderer>().color = Color.white;
                }
                
                currentGrapple = grapple.transform;
                currentGrapple.GetComponent<SpriteRenderer>().color = Color.red;

                distance = currentDistance;

            }
        }

        if(distance !=  maxTravelDistance + 0.1f)
        {
            return;
        }

        currentGrapple.GetComponent<SpriteRenderer>().color = Color.white;
        currentGrapple = null;
    }
}
