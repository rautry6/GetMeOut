using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityTrap : MonoBehaviour
{
    [SerializeField] private GameObject point1;
    [SerializeField] private GameObject point2;

    private LineRenderer lineRenderer;
    [SerializeField] private EdgeCollider2D edgeCollider;
    [SerializeField] private EdgeCollider2D edgeCollider2;

    [SerializeField] private float offSpeed = 2f;
    [SerializeField] private float onSpeed = 4f;
    private float timer = 0f;
    private bool on = false;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        edgeCollider.SetPoints(new List<Vector2> { point1.transform.position, point2.transform.position });
        edgeCollider.enabled = false;

        edgeCollider2.SetPoints(new List<Vector2> { point2.transform.position, point1.transform.position });
        edgeCollider2.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0f)
        {
            FlipOnState();
            
            if(on)
            {
                timer = offSpeed;
            }
            else
            {
                timer = onSpeed;
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void FlipOnState()
    {
        on = !on;

        if(on)
        {
            lineRenderer.enabled = true;
            edgeCollider.enabled = true;
            edgeCollider2.enabled=true;

            lineRenderer.SetPosition(0, point1.transform.position);
            lineRenderer.SetPosition(1, point2.transform.position);

            edgeCollider.SetPoints(new List<Vector2> { point1.transform.position, point2.transform.position });
            edgeCollider2.SetPoints(new List<Vector2> { point2.transform.position, point1.transform.position });
        }
        else
        {
            lineRenderer.enabled = false;
            edgeCollider.enabled = false;
            edgeCollider2.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage();
        }
    }
}
