using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoreText", menuName = "Items/LoreText")]
public class LoreSO : ScriptableObject
{
    private string _loreText;
    public string LoreText
    {
        get => _loreText;
        set => _loreText = value;
    }
}
