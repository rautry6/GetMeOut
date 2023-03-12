using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{

    [Header("Sway")]
    [SerializeField] private float swayRotation;
    [SerializeField, Tooltip("Higher value == slower swing")] private float swaySpeed = 0.1f;
    [SerializeField] private bool canSway = true;
    
    private float currentRotation = 0;
    private bool swayRight = true;
    private bool swaying = false;

    [Header("Fall"), Tooltip("How far away from the trap the player has to be for it to fall")]
    [SerializeField] private float fallRange = 0.5f;
    [SerializeField] private float fallHeight;

    private GameObject player;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody2D trapRigidBody;

    [Header("Damage"), Tooltip("True when the trap is hanging. False when it hits the ground so the player can walk through it.")]
    [SerializeField] private bool canDamage = true;

    [SerializeField] private Collider2D triggerCollider;

    [Header("Animation")]
    [SerializeField] private Sprite[] swayingAnimation;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private int currentSprite = 3;
    private Animator animator;

    [Header("Falling")]
    [SerializeField] private GameObject fallingObject;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        //spriteRenderer.sprite = swayingAnimation[currentSprite];
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggerCollider.enabled = false;
            animator.SetBool("Fall", true);
        }
    }

    public void Fall()
    {
        Instantiate(fallingObject, transform);
    }
}
