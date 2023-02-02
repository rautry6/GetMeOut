using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyCardManager : MonoBehaviour
{
    private List<KeyCard> keyCardsCollected;

    private void Start()
    {
        keyCardsCollected = new List<KeyCard>();
    }

    public void OnKeyCardCollected(KeyCard keyCard)
    {
        Debug.Log("OnKeyCardCollected");
        keyCardsCollected.Add(keyCard);
    }

    public bool CheckIfManagerHasCorrectCard(KeyCardColors keyCard)
    {
        return keyCardsCollected.Find(x => x.CardColor == keyCard);
    }
}
