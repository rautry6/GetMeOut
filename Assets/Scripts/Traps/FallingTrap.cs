using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{

    [Header("Sway")]
    [SerializeField] private float swayRotation;
    [SerializeField] private float rotateAmount = 0.1f;
    
    private float currentRotation = 0;
    private bool swayRight = true;

    [Header("Fall"), Tooltip("How far away from the trap the player has to be for it to fall")]
    [SerializeField] private float fallRange = 0.5f;

    private GameObject player;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody2D trapRigidBody;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (swayRight)
        {
            currentRotation += rotateAmount;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotation);

            if(currentRotation > swayRotation)
            {
                swayRight = false;
            }
        }
        else
        {
            currentRotation -= rotateAmount;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotation);

            if (currentRotation < -swayRotation)
            {
                swayRight = true;
            }
        }

        if (player.transform.position.x > transform.position.x - fallRange && player.transform.position.x < transform.position.x + fallRange)
        {
            trapRigidBody.gravityScale = 1;
        }
    }
}
