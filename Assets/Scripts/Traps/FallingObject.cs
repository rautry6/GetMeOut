using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [Header("Rigidbody")]
    [SerializeField] private Rigidbody2D trapRigidBody;

    [Header("Damage"), Tooltip("True when the trap is hanging. False when it hits the ground so the player can walk through it.")]
    [SerializeField] private bool canDamage = true;

    private Collider2D collider1;
    private GameObject player;
    private Animator ac;

    // Start is called before the first frame update
    void Start()
    {
        trapRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        collider1 = GetComponent<Collider2D>();
        ac = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            //Makes trap stick into floor
            trapRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            canDamage = false;
            collider1.enabled = false;
            ac.SetBool("Falling", false);
        }

        if (collision.tag == "Player" && canDamage)
        {
            player.GetComponent<PlayerHealth>().TakeDamage();
        }
    }
}
