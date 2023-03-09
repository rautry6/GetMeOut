using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{

    [Header("Sway")]
    [SerializeField] private float swayRotation;
    [SerializeField] private float rotateAmount = 0.1f;
    [SerializeField] private bool canSway = true;
    
    private float currentRotation = 0;
    private bool swayRight = true;

    [Header("Fall"), Tooltip("How far away from the trap the player has to be for it to fall")]
    [SerializeField] private float fallRange = 0.5f;
    [SerializeField] private float fallHeight;

    private GameObject player;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody2D trapRigidBody;

    [Header("Damage"), Tooltip("True when the trap is hanging. False when it hits the ground so the player can walk through it.")]
    [SerializeField] private bool canDamage = true;

    [SerializeField] private Collider2D triggerCollider;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (swayRight && canSway)
        {
            currentRotation += rotateAmount;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotation);

            if(currentRotation > swayRotation)
            {
                swayRight = false;
            }
        }
        else if(canSway)
        {
            currentRotation -= rotateAmount;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotation);

            if (currentRotation < -swayRotation)
            {
                swayRight = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground" && !canSway)
        {
            //Makes trap stick into floor
            trapRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            canDamage = false;
        }


        if (collision.tag == "Player")
        {
            Debug.Log("player");
            if (canSway)
            {
                triggerCollider.enabled = false;
                trapRigidBody.gravityScale = 1;
                canSway = false;
            }
            else if (canDamage)
            {
                player.GetComponent<PlayerHealth>().TakeDamage();
            }
        }
    }
}
