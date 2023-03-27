using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [SerializeField, Tooltip("Title HUD")]
    GameObject titleHUD;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            titleHUD.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
