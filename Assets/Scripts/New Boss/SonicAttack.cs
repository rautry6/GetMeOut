using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicAttack : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float destroyTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * moveSpeed * Time.deltaTime;

        destroyTimer  -= Time.deltaTime;

        if (destroyTimer < 0)
        {
            Destroy(gameObject);
        }
    }
}
