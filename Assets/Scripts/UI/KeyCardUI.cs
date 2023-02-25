using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class KeyCardUI : MonoBehaviour
{
    [SerializeField] private GameObject keycardholder;
    [SerializeField] private GameObject keyCardUIElement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddKeyCardUI(KeyCard keyCard)
    {
        Color keyColor = keyCard.CardColorValue;

        GameObject g = Instantiate(keyCardUIElement, keycardholder.transform);

        g.GetComponent<Image>().color = keyColor;
        //keyCardUIElement.GetComponent<Image>().sprite = keyCard.KeyCardSprite;

    }
}
