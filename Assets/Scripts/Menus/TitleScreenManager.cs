using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Number of Choices"), SerializeField, Tooltip("Number of choices in the list")]
    int numChoices = 3;

    [Header("Spacing"), SerializeField, Tooltip("Number of pixels between y values of text")]
    float spacing = 33;

    private GameObject _ptr;
    private float _ptrStartY;
    private int _selected;
    
    void Start()
    {
        _ptr = GameObject.FindGameObjectWithTag("Pointer");
        _ptrStartY = _ptr.transform.position.y;
        _selected = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (_ptr.transform.position.y > (_ptrStartY + ((numChoices - 1) * -spacing)))
            {
                _ptr.transform.position = _ptr.transform.position + new Vector3(0, -spacing, 0);
                _selected++;
                //Debug.Log("Went down");
            }
            else
            {
                _ptr.transform.position = new Vector3(_ptr.transform.position.x, _ptrStartY, _ptr.transform.position.z);
                _selected = 1;
                //Debug.Log("Looped to top");
            }   
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (_ptr.transform.position.y < _ptrStartY)
            {
                _ptr.transform.position = _ptr.transform.position + new Vector3(0, spacing, 0);
                _selected--;
                //Debug.Log("Went up");
            }
            else
            {
                _ptr.transform.position = new Vector3(_ptr.transform.position.x, _ptrStartY + ((numChoices - 1) * -spacing), _ptr.transform.position.z);
                _selected = numChoices;
                //Debug.Log("Looped to bottom");
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (_selected == 1) //Start
            {
                SceneManager.LoadScene("StartingLevel");
            } else if (_selected == 2) //Options
            {

            } else if (_selected == 3) //Quit
            {

            }
        }
    }
}
