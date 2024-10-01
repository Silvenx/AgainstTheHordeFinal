using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : CharacterManager
{

    [Header("Horde Specific")]
    public PlayerManager playerManager;

    public void GameSetup(DeckObjects deck)
    {
        //Populate Deck in game & Shuffle
        myDeck = new Deck(deck);
    }

    public void HordePlayFromDeck()
    {
        //Get Card From Top of Deck
        Card card = myDeck.TakeTopCard();
        Debug.Log("HordeCardName: " + card.cardName);
        //play the card (checking what type it is)
        PlayHordeCard(card);
    }

    private void PlayHordeCard(Card card)
    {
        //Create Card Object
        GameObject cardObject = gameManager.CreateCardObject(card);

        //JP 28.09.24 - Added if on monster
        //FUTURE: Does this need to be case?
        if (card.cardType == Card.CARDTYPE.MONSTER)
        {
            //Calls the method to find gameslots available against a player and then just free
            List<GameObject> applicableFieldSlots = fieldManager.ApplicableFieldSlotsToPlay(false, card);

            //If a free slot is found it's played here
            if (applicableFieldSlots.Count > 0)
            {
                cardObject.GetComponent<CardDetails>().PlayThisCardOnFieldSlot(applicableFieldSlots[0]);
                Debug.Log($"Horde card placed in slot {applicableFieldSlots[0].name}");
            }

            //If none are found then the card is discarded
            else
            {
                Debug.Log("All horde slots are full. Sending card to graveyard.");
                FieldManager.SendCardObjectToGraveyard(cardObject, false);
            }
        }
        //JP 28.09.24 - Added in field play
        else if (card.cardType == Card.CARDTYPE.FIELD)
        {
            List<GameObject> applicableFieldSlots = fieldManager.ApplicableFieldSlotsToPlay(false, card);
            cardObject.GetComponent<CardDetails>().PlayThisCardOnFieldSlot(applicableFieldSlots[0]);
        }
        else
        {
            //JP 28.09.24 - Discard the card
            //FUTURE: Add spell effects here, probably again in case
            FieldManager.SendCardObjectToGraveyard(cardObject, false);
        }

    }

    public Card getTopCardFromDeck(int amountToGet)
    {
        return myDeck.TakeTopCard();
    }
    public Card getTopCardFromDeck()
    {
        return myDeck.TakeTopCard();
    }

    public void DamageHordeLifeforce(int damage)
    {
        ModifyCharacterLifeForce(-damage);
    }

    public void HealHordeLifeforce(int healAmount)
    {
        ModifyCharacterLifeForce(healAmount);
    }

    public void SetHordeLifeForce(int amountToSet)
    {
        SetCharacterLifeForce(amountToSet);
    }

}
