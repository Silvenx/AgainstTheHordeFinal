using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Managers")]
    public GameManager gameManager;
    public FieldManager fieldManager;
    public float timeForCardsToMove = 1.5f;
    public float cardMoveSpeed;

    [Header("PlayerDeck")]
    public GameObject deckGameObject;
    public Deck playerDeck;
    [Header("PlayerHand")]
    public GameObject handThreshholdArea;
    public GameObject handParentObject;
    public List<GameObject> playerHand;
    public int startingHandSize = 5; //value must be greater than 2
    public int maxHandSize = 10;
    public float distanceBetweenCardsInHand;
    public GameObject cardBeingMoved;


    

    public void GameSetup(Deck deck)
    {
        //Populate Deck in game
        playerDeck = deck;
    }
    public void GameSetup(DeckObjects deck)
    {
        //Populate Deck in game & Shuffle
        playerDeck = new Deck(deck);

        //Draw Cards to Hand from top of deck
        DrawCardsFromTopOfDeck(startingHandSize-2); //minus 1 because method after ensures next draw is a 1 cost
                                                    //minus another 1 because turnphase will trigger another card draw
        //Draws 1 card. If 1 cost card doesn't exist in player's hand, will force draw a random 1 cost from deck
        DrawCardEnsuringOneCostCard();

    }
    //Checks hand if 1 cost card exists, if not draws a 1 cost card, if so draws top card
    private void DrawCardEnsuringOneCostCard()
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
        else
        {
            //Draw a card with a 1 cost
            DrawCardToHand(playerDeck.FindCard(1));
        }
    }

    //Draws Multiple Cards
    public void DrawCardsFromTopOfDeck(int amountToDraw)
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            //Get Card from top of deck & draw it
            DrawCardToHand(playerDeck.DrawTopCard());
        }
    }
    //TEST SCRIPT
    public void DrawCardFromTopOfDeck()
    {
        DrawCardToHand(playerDeck.DrawTopCard());
    }
    private void PutCardInHand(Card card)
    {

    }


    private List<GameObject> getCardPositions()
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

        //If hand not equal to or above max hand size
        if (playerHand.Count < maxHandSize)
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
            //try
            //{
            //    //Stops current Coroutine
            //    StopCoroutine(playerHand[i].GetComponent<CardDetails>().currentCoroutine);
            //    playerHand[i].GetComponent<CardDetails>().currentCoroutine = null;
            //}
            //catch (System.NullReferenceException e) { }

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

    ////To be used when mousing over card that is in player's hand
    //public void LerpCardUpwardsSlightly(GameObject cardObject)
    //{
    //    //If card does infact exist in hand
    //    if (playerHand.Contains(cardObject))
    //    {
    //        try
    //        {
    //            //ends current coroutine if one is occuring
    //            StopCoroutine(cardObject.GetComponent<CardDetails>().currentCoroutine);
    //        }
    //        catch (System.NullReferenceException e) { }

    //        //Get target positions for all cards
    //        List<Vector2> oldCardPositions = CalculateCardPositionsForHand(playerHand.Count);
    //        //Find where in list this card exists
    //        int i = playerHand.IndexOf(cardObject);
    //        //Figure out new position of specific card being moved
    //        Vector2 targetPos = new Vector2(oldCardPositions[i].x, oldCardPositions[i].y + gameManager.highlightCardRaiseDistance);

    //        //Starts Coroutine
    //        playerHand[i].GetComponent<CardDetails>().currentCoroutine = LerpObjectMovement(cardObject, oldCardPositions[i], targetPos, cardMoveSpeed);
    //        StartCoroutine(cardObject.GetComponent<CardDetails>().currentCoroutine);
    //    }
    //}

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

    IEnumerator WaitSeconds(float secToWait)
    {
        Debug.Log("Waiting " + secToWait + " seconds.");
        yield return new WaitForSeconds(secToWait);
    }
    //Lerps object from 1 pos to another with a soft finish (sin graph)
    IEnumerator LerpObjectMovement(GameObject objectToMove, Vector3 startPos, Vector3 endPos, float timeToMove)
    {
        float currentLerpTime = 0;
        while (currentLerpTime <= timeToMove) //until X sec passed
        {
            currentLerpTime += Time.deltaTime * cardMoveSpeed;
            float perc = currentLerpTime / timeToMove;
            perc = Mathf.Sin(perc * Mathf.PI * 0.5f);
            objectToMove.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(startPos, endPos, perc);

            //End early if reached position
            if (Vector3.Distance(objectToMove.transform.position, endPos) <= 0.01f)
            {
                objectToMove.GetComponent<CardDetails>().currentCoroutine = null;
                break;
            }

            yield return 1; //wait for next frame
        }
        objectToMove.GetComponent<CardDetails>().currentCoroutine = null;
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

    public void RemoveCardFromHand(GameObject card)
    {
        if (playerHand.Contains(card))
        {
            playerHand.Remove(card);

            OrganiseHand(getCardPositions());
        }
    }
}
