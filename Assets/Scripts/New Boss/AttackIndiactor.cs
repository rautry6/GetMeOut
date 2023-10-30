using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndiactor : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 4f;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        destroyDelay -= Time.deltaTime;
        if(destroyDelay < 0)
        {
            Destroy(gameObject);
        }
    }
}
