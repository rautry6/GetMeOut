using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config Instance { get; private set; }

    [Header("Player")]
    public float FootstepInterval = 1f;


    // Start is called before the first frame update
    void Awake()
    {
        //Destroys the extra Config if any are found
        if (Instance != null)
        {
            Debug.LogError("Multiple Configs found. Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
