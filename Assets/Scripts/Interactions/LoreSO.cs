using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoreText", menuName = "Items/LoreText")]
public class LoreSO : ScriptableObject
{
    [SerializeField]
    private string loreText = "";
    public string LoreText => loreText;
}
