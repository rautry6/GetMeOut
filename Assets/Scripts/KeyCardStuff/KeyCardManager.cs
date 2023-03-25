using System.Collections.Generic;
using UnityEngine;

public class KeyCardManager : MonoBehaviour
{
    private List<KeyCard> keyCardsCollected;
    [SerializeField] private KeyCardUI keyCardUI;
    [SerializeField] private KeyCard redCard;
    [SerializeField] private KeyCard whiteCard;
    [SerializeField] private KeyCard greenCard;
    [SerializeField] private KeyCard purpleCard;
    [SerializeField] private KeyCard blueCard;
    [SerializeField] private KeyCard yellowCard;

    private void Awake()
    {
        keyCardsCollected = new List<KeyCard>();
    }

    public void OnKeyCardCollected(KeyCard keyCard)
    {
        Debug.Log("OnKeyCardCollected");
        keyCardsCollected.Add(keyCard);
        AutoSave.Instance.AddKeyCard(keyCard.name);
    }

    public bool CheckIfManagerHasCorrectCard(KeyCardColors keyCard)
    {
        return keyCardsCollected.Find(x => x.CardColor == keyCard);
    }

    public void LoadKeyCards()
    {
        foreach (var keyCard in AutoSave.Instance.KeyCards)
        {
            switch (keyCard)
            {
                case "KeyCard_White":
                {
                    if (keyCardsCollected.Contains(whiteCard)) continue;
                    keyCardsCollected.Add(whiteCard);
                    keyCardUI.AddKeyCardUI(whiteCard);
                    break;
                }
                case "KeyCard_Red":
                {
                    if (keyCardsCollected.Contains(redCard)) continue;
                    keyCardsCollected.Add(redCard);
                    keyCardUI.AddKeyCardUI(redCard);
                    break;
                }
                case "KeyCard_Green":
                {
                    if (keyCardsCollected.Contains(greenCard)) continue;
                    keyCardsCollected.Add(greenCard);
                    keyCardUI.AddKeyCardUI(greenCard);
                    break;
                }
                case "KeyCard_Purple":
                {
                    if (keyCardsCollected.Contains(purpleCard)) continue;
                    keyCardsCollected.Add(purpleCard);
                    keyCardUI.AddKeyCardUI(purpleCard);
                    break;
                }
                case "KeyCard_Blue":
                {
                    if (keyCardsCollected.Contains(blueCard)) continue;
                    keyCardsCollected.Add(blueCard);
                    keyCardUI.AddKeyCardUI(blueCard);
                    break;
                }
                case "KeyCard_Yellow":
                {
                    if (keyCardsCollected.Contains(yellowCard)) continue;
                    keyCardsCollected.Add(yellowCard);
                    keyCardUI.AddKeyCardUI(yellowCard);
                    break;
                }
            }
            
        }
    }
}