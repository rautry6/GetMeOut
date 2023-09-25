using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class UpAttackEnemy : MonoBehaviour
{
    [SerializeField] private Collider2D hitBox;
    [SerializeField] private Animator animator;
    [SerializeField] private float timeBetweenAttacks;
    private bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!attacking)
        {
            attacking = true;
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void AttackOver()
    {
        animator.SetBool("Attack", false);

        attacking = false;
    }
}
