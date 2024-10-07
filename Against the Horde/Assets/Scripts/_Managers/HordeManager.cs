using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : CharacterManager
{

    [Header("Horde Specific")]
    public PlayerManager playerManager;
    public CardDetails cardDetails;

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
        //Select Card Object
        GameObject cardObject = gameManager.CreateCardObject(card);
        var cardDetails = cardObject.GetComponent<CardDetails>();

        //JP 07.10.24 - Adjusted to Case statement
        switch (card.cardType)
        {
            case Card.CARDTYPE.MONSTER:
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
                break;

            case Card.CARDTYPE.SPELL:
                //Play the spell
                //THIS IS CURRENTLY FAILING - i think it's on targeting? It is running through though and not crashing the game.
                Debug.Log($"Card details: {cardDetails}");
                Debug.Log($"Card Object: {cardObject}");
                Debug.Log($"Card Details: {cardObject?.GetComponent<CardDetails>()}");
                this.cardDetails.ActivateCardEffect(TriggerType.PLAY);
                //FUTURE: Allow for reaction time
                //Discard the spell
                FieldManager.SendCardObjectToGraveyard(cardObject, false);
                break;


            case Card.CARDTYPE.FIELD:
                List<GameObject> applicableFieldCardSlot = fieldManager.ApplicableFieldSlotsToPlay(false, card);
                cardObject.GetComponent<CardDetails>().PlayThisCardOnFieldSlot(applicableFieldCardSlot[0]);
                break;
        }
    }

    /*
            if (card.cardType == Card.CARDTYPE.MONSTER)
            {
                //Calls the method to find gameslots available against a player and then just free
                List<GameObject> applicableFieldSlots = fieldManager.ApplicableFieldSlotsToPlay(false, card);

                //If a free slot is found it's played here
                if (applicableFieldSlots.Count > 0)
                {
                    cardObject.GetComponent<CardDetails>().PlayThisCardOnFieldSlot(applicableFieldSlots[0]);
                    Debug.Log($"Horde card placed in slot {applicableFieldSlots[0].name}");
                    //cardObject.GetComponent<CardDetails>().ActivateCardEffect(TriggerType.PLAY);//JP 03.10.24 - This is somewhere else, oops. 
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
            else if (card.cardType == Card.CARDTYPE.SPELL)
            {

            }
            else
            {
                //JP 28.09.24 - Discard the card
                //FUTURE: Add spell effects here, probably again in case
                FieldManager.SendCardObjectToGraveyard(cardObject, false);
            }

        }
    */
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
