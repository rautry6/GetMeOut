using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyCardColors
{
    Red,
    Green,
    Blue,
    Yellow
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
