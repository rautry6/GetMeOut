using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDebris : MonoBehaviour
{
    Rigidbody2D debrisRigidbody;
    [SerializeField] private Collider2D collider1;
    // Start is called before the first frame update
    void Start()
    {
        debrisRigidbody = GetComponent<Rigidbody2D>();
        collider1 = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fall()
    {
        debrisRigidbody.gravityScale = 1;
    }

    public void Destroy()
    {
        //Play animation and delete
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            //Makes trap stick into floor
            debrisRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            //collider1.enabled = false;
            HearingManager.Instance.OnSoundEmitted(transform.position, HearingManager.EHeardSoundCategory.ECrash, 100f);
        }
    }
}
