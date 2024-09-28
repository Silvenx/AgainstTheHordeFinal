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
        Card card = myDeck.DrawTopCard();
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
            List<GameObject> applicableFieldSlots = fieldManager.ApplicableFieldSlotsToPlay(false, card);

            //For each applicable monster slot
            for (int i = 0; i < applicableFieldSlots.Count; i++)
            {
                //Look at each field slot in order of 1st to last
                if (applicableFieldSlots[i].transform.childCount == 0)
                {

                    //Play this card the first field slot applicable
                    cardObject.GetComponent<CardDetails>().PlayThisCardOnFieldSlot(applicableFieldSlots[i]);
                    break;
                }
                else
                {
                }
            }
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
        return myDeck.DrawTopCard();
    }
    public Card getTopCardFromDeck()
    {
        return myDeck.DrawTopCard();
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
