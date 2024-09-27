using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [Header("Player Specific")]
    public HordeManager hordeManager;
    [Header("My Hand")]
    public GameObject handParentObject;
    public List<GameObject> playerHand;
    public int startingHandSize = 5; //value must be greater than 2
    public int maxHandSize = 12;
    public float distanceBetweenCardsInHand = 35f;
    [HideInInspector]
    public GameObject cardBeingMoved;


    public void PlayerGameSetup(DeckObjects deck)
    {
        //Populate Deck in game & Shuffle
        myDeck = new Deck(deck);

        //Draw Cards to Hand from top of deck
        DrawCardsFromTopOfDeck(startingHandSize - 2); //minus 1 because method after ensures next draw is a 1 cost
                                                      //minus another 1 because turnphase will trigger another card draw
                                                      //Draws 1 card. If 1 cost card doesn't exist in player's hand, will force draw a random 1 cost from deck
        DrawCardEnsuringOneCostCard();

    }
    //Checks hand if 1 cost card exists, if not draws a 1 cost card, if so draws top card
    protected void DrawCardEnsuringOneCostCard()
    {
        //Check to see if hand has a 1 cost hand
        bool oneCostCardExistsInHand = false;
        foreach (GameObject o in playerHand)
        {
            //if this card is a 1 cost
            if (o.GetComponent<CardDetails>().card.baseManaCost == 1)
            {
                oneCostCardExistsInHand = true;
                break;
            }
        }
        //If 1 cost card exists in hand
        if (oneCostCardExistsInHand)
        {
            //Draw 1 card
            DrawCardFromTopOfDeck();
        }
        //If no 1 cost cards in  hand
        //JP 27.09.24 - Adjusted to remove the old card from the deck 
        else
        {
            //Draw a card with a 1 cost
            Card oneCostCard = myDeck.FindCard(1);
            if (oneCostCard != null)
            {
                DrawCardToHand(oneCostCard);
                //Removes card from the deck list
                myDeck.getDeck().Remove(oneCostCard);
            }

        }
    }

    //Draws Multiple Cards
    public void DrawCardsFromTopOfDeck(int amountToDraw)
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            //Get Card from top of deck & draw it
            DrawCardToHand(myDeck.DrawTopCard());
        }
    }
    //TEST SCRIPT
    public void DrawCardFromTopOfDeck()
    {
        DrawCardToHand(myDeck.DrawTopCard());
    }
    public void PutCardInHand(Card card)
    {

    }


    protected List<GameObject> getCardPositions()
    {
        List<GameObject> list = new List<GameObject>();
        try
        {
            foreach (GameObject o in playerHand)
            {
                list.Add(o);
            }
        }
        catch (System.NullReferenceException e) { }

        return list;
    }
    public void DrawCardToHand(Card cardToDraw)
    {

        //If hand not equal to or above max hand size AND deck was not empty
        if (playerHand.Count < maxHandSize && cardToDraw != null)
        {
            //Get Positions of all existing card in hand
            List<GameObject> oldCardPositions = getCardPositions();

            //Get Card Game Object (from object pool)
            GameObject cardObject = gameManager.RetrieveCardObject(cardToDraw);
            //Populate Card's details on Game Object
            cardObject.GetComponent<CardDetails>().SetCardDetails(cardToDraw);

            //Set Transfor.Parent to be CardGroup
            cardObject.transform.SetParent(handParentObject.transform);
            //MAY NEED TO CHANGE RECTTRANSFORM PIVOT & ETC.
            ////Move card's physical position to top of deck
            cardObject.GetComponent<RectTransform>().anchoredPosition = deckGameObject.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition;
            //Add card to hand list
            playerHand.Add(cardObject);

            //StartCoroutine(LerpBetweenLocations(cardObject, cardObject.transform.position, handParentObject.transform.position, timeForCardsToMove));

            //Spaces out cards in hand & moves drawn card to hand if one was drawn
            OrganiseHand(oldCardPositions);
        }
    }
    public GameObject deckPositionRelativeToHand;
    //Spreads out cards in hand
    public void OrganiseHand()
    {
        OrganiseHand(getCardPositions());
    }
    public void OrganiseHand(bool moveInstantly)
    {
        //Move with lerp
        if (!moveInstantly)
        {
            OrganiseHand(getCardPositions());
        }
        //Move Instantly
        else
        {

            OrganiseHandInstantly(getCardPositions());

        }
    }
    public void OrganiseHand(float cardSpeed)
    {
        OrganiseHand(getCardPositions(), cardMoveSpeed);
    }

    private void OrganiseHand(List<GameObject> oldCardPositions)
    {
        OrganiseHand(oldCardPositions, timeForCardsToMove);
    }

    private void OrganiseHandInstantly(List<GameObject> oldCardPositions)
    {
        int handCount = playerHand.Count;
        //skip organising, no cards in hand
        if (handCount == 0) { return; }

        //Find new positions to go to
        List<Vector2> newCardPositions = CalculateCardPositionsForHand(handCount);

        //Lerp to new positions from old
        for (int i = 0; i < handCount; i++)
        {
            try
            {
                //Stops current Coroutine
                StopCoroutine(playerHand[i].GetComponent<CardDetails>().currentCoroutine);
                playerHand[i].GetComponent<CardDetails>().currentCoroutine = null;
            }
            catch (System.NullReferenceException e) { }

            Vector2 oldPos;
            try { oldPos = oldCardPositions[i].GetComponent<RectTransform>().anchoredPosition; }
            //Hand size increasing, manually set it's "start position" for the Lerp
            catch (System.ArgumentOutOfRangeException e)
            { oldPos = deckPositionRelativeToHand.GetComponent<RectTransform>().anchoredPosition; }

            Vector2 newPos = newCardPositions[i];
            //Move Card
            playerHand[i].GetComponent<RectTransform>().anchoredPosition = newPos;
        }
    }

    private void OrganiseHand(List<GameObject> oldCardPositions, float cardSpeed)
    {
        int handCount = playerHand.Count;
        //skip organising, no cards in hand
        if (handCount == 0) { return; }

        //Find new positions to go to
        List<Vector2> newCardPositions = CalculateCardPositionsForHand(handCount);

        //Lerp to new positions from old
        for (int i = 0; i < handCount; i++)
        {
            Vector2 oldPos;
            try { oldPos = oldCardPositions[i].GetComponent<RectTransform>().anchoredPosition; }
            //Hand size increasing, manually set it's "start position" for the Lerp
            catch (System.ArgumentOutOfRangeException e)
            { oldPos = deckPositionRelativeToHand.GetComponent<RectTransform>().anchoredPosition; }

            Vector2 newPos = newCardPositions[i];
            //StartCoroutine(LerpObjectMovement(playerHand[i], oldPos, newPos, cardSpeed));
            playerHand[i].GetComponent<CardDetails>().currentCoroutine = LerpObjectMovement(playerHand[i], oldPos, newPos, cardSpeed);
            StartCoroutine(playerHand[i].GetComponent<CardDetails>().currentCoroutine);

        }
    }

    private List<Vector2> CalculateCardPositionsForHand(int handCount)
    {
        List<Vector2> newCardPositions = new List<Vector2>();
        for (int i = 0; i < handCount; i++)
        {
            float xPos = ((float)handCount / 2) * -1 * distanceBetweenCardsInHand + distanceBetweenCardsInHand / 2
                + distanceBetweenCardsInHand * i;
            Vector2 v = new Vector2(xPos, 0);
            newCardPositions.Add(v);
        }
        return newCardPositions;
    }

    public void RemoveCardFromHand(GameObject card)
    {
        if (playerHand.Contains(card))
        {
            playerHand.Remove(card);

            OrganiseHand(getCardPositions());
        }
    }


    public void StopLerpCoroutineOnCard(GameObject ob)
    {
        if (playerHand.Contains(ob))
        {
            try
            {
                StopCoroutine(ob.GetComponent<CardDetails>().currentCoroutine);
            }
            catch (System.NullReferenceException e) { }
            ob.GetComponent<CardDetails>().currentCoroutine = null;
        }
    }
}
