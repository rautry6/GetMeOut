using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class KeyCardUI : MonoBehaviour
{
    [SerializeField] private GameObject keycardholder;
    [SerializeField] private GameObject keyCardUIElement;
    private List<Color> SpawnedColors = new List<Color>();
    public void AddKeyCardUI(KeyCard keyCard)
    {
        if (SpawnedColors.Contains(keyCard.CardColorValue)) return;
        
        Color keyColor = keyCard.CardColorValue;
        SpawnedColors.Add(keyColor);
        GameObject g = Instantiate(keyCardUIElement, keycardholder.transform);
        g.GetComponent<Image>().color = keyColor;
        //keyCardUIElement.GetComponent<Image>().sprite = keyCard.KeyCardSprite;

    }
}
