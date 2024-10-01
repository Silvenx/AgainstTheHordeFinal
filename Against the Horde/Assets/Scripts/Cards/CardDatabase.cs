using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Card Database")]
public class CardDatabase : ScriptableObject
{
    public List<CardObjects> allCards;

    // Optional: Method to get a card by name
    public Card GetCardByName(string cardName)
    {
        foreach (CardObjects cardObj in allCards)
        {
            if (cardObj.card.cardName == cardName)
            {
                return cardObj.card;
            }
        }
        Debug.LogError($"Card with name {cardName} not found in CardDatabase.");
        return null;
    }

    public Card GetCard(Card card)
    {
        foreach (CardObjects cardObj in allCards)
        {
            if(cardObj.card == card)
            {
                return cardObj.card;
            }
        }
        Debug.LogError($"Card with name {card.cardName} not found in CardDatabase.");
        return null;
    }
}
