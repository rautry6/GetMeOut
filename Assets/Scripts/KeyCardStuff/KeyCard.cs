using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyCardColors
{
    Red, // 0
    Green, // 1
    Blue, // 2
    Yellow, // 3
    Purple,
    Black,
    White,
    Orange,
    Brown,
}

[CreateAssetMenu(fileName = "KeyCard_COLOR", menuName = "Items/KeyCard")]
public class KeyCard : ScriptableObject
{
    [SerializeField] private KeyCardColors cardColor;
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private Color cardColorValue;
    
    public KeyCardColors CardColor => cardColor;
    public Sprite KeyCardSprite => cardSprite;
    public Color CardColorValue => cardColorValue;
}
