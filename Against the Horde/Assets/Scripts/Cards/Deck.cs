using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck
{
    [SerializeField]
    private List<Card> cardList;

    //-------------------------Initialize Deck-------------------------//
    //Initialize with list of Card objects
    public Deck(List<Card> cardList)
    {
        this.cardList = cardList;

        ShuffleDeck();
    }
    //Initialize with list of CardObjects scriptable objects
    public Deck(List<CardObjects> cardList)
    {
        //Empty out current list
        this.cardList = ConvertCardObjectsToCards(cardList);

        ShuffleDeck();
    }
    //Initialize with a DeckObjects Scriptable object
    public Deck(DeckObjects deck)
    {
        this.cardList = ConvertCardObjectsToCards(deck.cardList);

        ShuffleDeck();
    }
    //Converts a List of Card GameObjects into a List of Cards
    private List<Card> ConvertCardObjectsToCards(List<CardObjects> cardList)
    {
        //Creates empty list
        List<Card> list = new List<Card>();

        //Goes through supplied list & adds to new list
        foreach (CardObjects cardObject in cardList)
        {
            list.Add(new Card(cardObject));
        }

        return list;
    }

    //---------------------------Drawing Cards--------------------------------------//

    public Card DrawTopCard()
    {
        try
        {
            //Looks at top card
            Card topCard = getTopCard();
            //Remove card from deck
            cardList.RemoveAt(0);

            //Gives card
            return topCard;
        }
        catch (System.NullReferenceException e) { return null; }
    }

    private Card getTopCard()
    {
        return cardList[0];
    }
    private List<Card> getTopCards(int number)
    {
        List<Card> topCards = new List<Card>();
        for (int i = 0; i < number; i++)
        {
            topCards.Add(cardList[i]);
        }
        return topCards;
    }

    //---------------------------Changing Deck--------------------------------------//


    //Gives list of cards in deck
    public List<Card> getDeck()
    {
        return cardList;
    }
    public int getDeckSize()
    {
        return cardList.Count;
    }
    public Card FindCard(string cardName)
    {
        return cardList.Find(x => x.cardName.Contains(cardName));
    }
    public Card FindCard(int manaCost)
    {
        return cardList.Find(x => x.baseManaCost == manaCost);
    }

    //Adds one card to deck, then shuffles
    public void AddToDeck(Card card)
    {
        cardList.Add(card);

        ShuffleDeck();
    }
    //Adds multiple cards to deck, then shuffles
    public void AddToDeck(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            cardList.Add(card);
        }
        ShuffleDeck();
    }

    //Shuffles this Deck
    public void ShuffleDeck()
    {
        List<Card> newDeck = new List<Card>();
        List<Card> remainingCards = cardList;

        while (remainingCards.Count > 0)
        {
            //Get random number based on current selection pool
            int i = Random.Range(1, remainingCards.Count + 1);
            //Get chosen card
            Card c = remainingCards[i - 1];
            //Add card to new deck
            newDeck.Add(c);
            //Remove it from selection pool
            remainingCards.Remove(c);
        }

        cardList = newDeck;
    }
}
