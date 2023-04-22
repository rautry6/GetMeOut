using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField, Tooltip("Options HUD")]
    GameObject optionsHUD;
    
    [Header("Text Elements"), SerializeField, Tooltip("Text options in menu")]
    List<GameObject> options = new List<GameObject>();

    [SerializeField] private GameObject comingSoonUI;

    private float _spacing;
    private GameObject _ptr;
    private float _ptrStartY;
    private int _selected;
    
    void Start()
    {
        _ptr = GameObject.FindGameObjectWithTag("Pointer");
        _ptrStartY = _ptr.transform.position.y;
        _selected = 1;

        if (options.Count > 1)
        {
            _spacing = Mathf.Abs(options[1].transform.position.y - options[0].transform.position.y);
            
        } else
        {
            _spacing = 0;
        }

        //Debug.Log(_spacing);
        //Debug.Log(options.Count);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (_ptr.transform.position.y > (_ptrStartY + ((options.Count - 1) * -_spacing)))
            {
                _ptr.transform.position = _ptr.transform.position + new Vector3(0, -_spacing, 0);
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
                _ptr.transform.position = _ptr.transform.position + new Vector3(0, _spacing, 0);
                _selected--;
                //Debug.Log("Went up");
            }
            else
            {
                _ptr.transform.position = new Vector3(_ptr.transform.position.x, _ptrStartY + ((options.Count - 1) * -_spacing), _ptr.transform.position.z);
                _selected = options.Count;
                //Debug.Log("Looped to bottom");
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (_selected == 1) //New game
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("StartingLevel");
                MusicManager.Instance.PlayMusic();
            } else if (_selected == 2) //Load
            {
                if(!comingSoonUI.activeInHierarchy)
                    comingSoonUI.SetActive(true);   
            } else if (_selected == 3) //Options
            {
                optionsHUD.SetActive(true);
                gameObject.SetActive(false);

            } else if (_selected == 4) //Quit
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }
    }
}
