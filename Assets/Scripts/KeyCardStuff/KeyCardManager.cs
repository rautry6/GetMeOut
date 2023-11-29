using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyCardManager : MonoBehaviour
{
    private HashSet<KeyCard> keyCardsCollected;
    [SerializeField] private KeyCardUI keyCardUI;
    [SerializeField] private KeyCard redCard;
    [SerializeField] private KeyCard whiteCard;
    [SerializeField] private KeyCard greenCard;
    [SerializeField] private KeyCard purpleCard;
    [SerializeField] private KeyCard blueCard;
    [SerializeField] private KeyCard yellowCard;

    private void Awake()
    {
        keyCardsCollected = new HashSet<KeyCard>();
    }

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        LoadKeyCards();
    }

    public void OnKeyCardCollected(KeyCard keyCard, GameObject keyCardHolder)
    {
        if (!keyCardsCollected.Contains(keyCard))
        {
            keyCardsCollected.Add(keyCard);
            AutoSave.Instance.AddKeyCard(keyCard.name);    
            keyCardHolder.GetComponent<KeyCardHolder>().AddKeyCardUI();
        }
    }

    public bool CheckIfManagerHasCorrectCard(KeyCardColors keyCard)
    {
        var collectedKeyCard = keyCardsCollected.FirstOrDefault(x => x.CardColor == keyCard);
        return collectedKeyCard != null;
    }

    public void LoadKeyCards()
    {
        {
            keyCardUI = GameObject.Find("KeyCardUIHolder").GetComponent<KeyCardUI>();
            switch (keyCard)
            {
                case "KeyCard_White":
                {
                    //if (keyCardsCollected.Contains(whiteCard)) continue;
                    keyCardsCollected.Add(whiteCard);
                    keyCardUI.AddKeyCardUI(whiteCard);
                    break;
                }
                case "KeyCard_Red":
                {
                    //if (keyCardsCollected.Contains(redCard)) continue;
                    keyCardsCollected.Add(redCard);
                    keyCardUI.AddKeyCardUI(redCard);
                    break;
                }
                case "KeyCard_Green":
                {
                    //if (keyCardsCollected.Contains(greenCard)) continue;
                    keyCardsCollected.Add(greenCard);
                    keyCardUI.AddKeyCardUI(greenCard);
                    break;
                }
                case "KeyCard_Purple":
                {
                    //if (keyCardsCollected.Contains(purpleCard)) continue;
                    keyCardsCollected.Add(purpleCard);
                    keyCardUI.AddKeyCardUI(purpleCard);
                    break;
                }
                case "KeyCard_Blue":
                {
                    //if (keyCardsCollected.Contains(blueCard)) continue;
                    keyCardsCollected.Add(blueCard);
                    keyCardUI.AddKeyCardUI(blueCard);
                    break;
                }
                case "KeyCard_Yellow":
                {
                    //if (keyCardsCollected.Contains(yellowCard)) continue;
                    keyCardsCollected.Add(yellowCard);
                    keyCardUI.AddKeyCardUI(yellowCard);
                    break;
                }
            }
            
        }
    }
}