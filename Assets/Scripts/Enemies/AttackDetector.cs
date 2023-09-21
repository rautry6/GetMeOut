using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetector : MonoBehaviour
{
    [SerializeField] PatrollingEnemy _patrrollingEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_patrrollingEnemy != null)
            {
                _patrrollingEnemy.Attack(collision.gameObject);
            }
        }
    }
}
