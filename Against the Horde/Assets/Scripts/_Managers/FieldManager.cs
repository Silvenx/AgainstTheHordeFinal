using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldManager : MonoBehaviour
{

    [Header("Managers")]
    public GameManager gameManager;
    public PlayerManager playerManager;

    [Header("CardField")]

    public GameObject playerMonsterParentObject;
    public List<GameObject> playerMonsterSlots;

    public GameObject hordeMonsterParentObject;
    public List<GameObject> hordeMonsterSlots;

    public GameObject fieldCardSlot;

    public List<GameObject> playerGraveyardList = new List<GameObject>();
    public List<GameObject> hordeGraveyardList = new List<GameObject>();
    public GameObject playerGraveyardSlot;
    public GameObject hordeGraveyardSlot;

    //--------------------------------------------------------------------------------------------------------------
    public List<GameObject> getAllPlayerMonsters()
    {
        List<GameObject> m = new List<GameObject>();

        //For each moster slot on the player's side...
        for (int i = 0; i < playerMonsterSlots.Count; i++)
        {
            //if the slot is not empty
            if (playerMonsterSlots[i].transform.childCount != 0)
            {
                m.Add(playerMonsterSlots[i].transform.GetChild(0).gameObject);
            }
        }

        return m;
    }
    public List<GameObject> getAllHordeMonsters()
    {
        List<GameObject> m = new List<GameObject>();

        //For each moster slot on the player's side...
        for (int i = 0; i < hordeMonsterSlots.Count; i++)
        {
            //if the slot is not empty
            if (hordeMonsterSlots[i].transform.childCount != 0)
            {
                m.Add(hordeMonsterSlots[i].transform.GetChild(0).gameObject);
            }
        }

        return m;
    }
    public (bool, int) getCardsFieldSlotPosition(GameObject cardObject)
    {
        //If card is on player's field
        if (isMonsterOnPlayerField(cardObject))
        {
            //Check all player slots for this card
            for (int i = 0; i < playerMonsterSlots.Count; i++)
            {
                //If fieldslot is not empty
                if (playerMonsterSlots[i].transform.childCount != 0)
                {
                    //If THE card in this slot
                    if (Object.ReferenceEquals(playerMonsterSlots[i].transform.GetChild(0).gameObject, cardObject))
                    {
                        return (true, i);
                    }
                }
            }
        }
        //If card is on player's field
        else if (isMonsterOnHordeField(cardObject))
        {
            //Check all horde slots for this card
            for (int i = 0; i < hordeMonsterSlots.Count; i++)
            {
                //If fieldslot is not empty
                if (hordeMonsterSlots[i].transform.childCount != 0)
                {
                    //If card in this slot
                    if (Object.ReferenceEquals(hordeMonsterSlots[i].transform.GetChild(0).gameObject, cardObject))
                    {
                        return (false, i);
                    }
                }
            }
        }

        return (false, 0);
    }
    public GameObject getMonsterAt(bool isPlayerMonsterField, int slotPosition)
    {
        GameObject toReturn = null;
        //Get from player monster field
        if (isPlayerMonsterField)
        {
            //If there is a card on this field slot position...
            Transform t = playerMonsterSlots[slotPosition].transform;
            if (t.childCount != 0)
            {
                //return that card object
                toReturn = t.GetChild(0).gameObject;
            }
        }
        //Get from horde monster field
        else if (!isPlayerMonsterField)
        {
            //If there is a card on this field slot position...
            Transform t = hordeMonsterSlots[slotPosition].transform;
            if (t.childCount != 0)
            {
                //return that card object
                toReturn = t.GetChild(0).gameObject;
            }
        }
        return toReturn;
    }
    public GameObject getFieldSlotAt(bool isPlayerMonsterField, int slotPosition)
    {
        //If wanting to get from player field...
        if (isPlayerMonsterField)
        {
            //Get slot at that position
            return playerMonsterSlots[slotPosition];
        }
        //If wanting to get from horde field...
        else //(!isPlayerMonsterField)
        {
            //Get slot at that position
            return hordeMonsterSlots[slotPosition];
        }
    }
    public bool isMonsterOnPlayerField(GameObject cardObject)
    {
        if (getAllPlayerMonsters().Contains(cardObject))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool isMonsterOnHordeField(GameObject cardObject)
    {
        if (getAllHordeMonsters().Contains(cardObject))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<GameObject> ApplicableFieldSlotsToPlay(bool isPlayerCard, Card card)
    {
        //JP 28.09.24 - Adjusted to use the allegiance on the card and check player slots

        List<GameObject> slots = new List<GameObject>();
        GameManager.TurnPhase currentTurnPhase = gameManager.currentTurn;

        switch (card.cardType)
        {
            case Card.CARDTYPE.MONSTER:
                //If Horde is playing
                if (card.cardAllegiance == Card.ALLEGIANCE.HORDE)
                {
                    //First try to play against a player monster
                    for (int i = 0; i < playerMonsterSlots.Count; i++)
                    {
                        //Set variables
                        GameObject playerSlot = playerMonsterSlots[i];
                        GameObject hordeSlot = hordeMonsterSlots[i];

                        //If both the player and horde slot are free
                        if (playerSlot.transform.childCount > 0 && hordeSlot.transform.childCount == 0)
                        {
                            //Add horde card to field and exit
                            slots.Add(hordeSlot);
                            return slots;
                        }
                    }
                    //Second try to play in any empty space
                    for (int i = 0; i < hordeMonsterSlots.Count; i++)
                    {
                        GameObject hordeSlot = hordeMonsterSlots[i];
                        //Check if the horde slot is empty
                        if (hordeSlot.transform.childCount == 0)
                        {
                            slots.Add(hordeSlot);
                            //Add horde card to field and exit
                            return slots;
                        }
                    }

                }
                //Player is playing
                else
                {
                    //For every slot in playermonster
                    foreach (GameObject slot in playerMonsterSlots)
                    {
                        //if the slot doesn't have a monster on it
                        if (slot.transform.childCount == 0)
                        {
                            slots.Add(slot);
                        }
                    }
                }
                break;

            case Card.CARDTYPE.SPELL:
                break;
            case Card.CARDTYPE.FIELD:
                //Add field card under the slot
                slots.Add(fieldCardSlot);
                //FUTURE: destroy the card that's already in there
                return slots;
            case Card.CARDTYPE.EQUIPMENT:
                break;
            case Card.CARDTYPE.ENCHANTMENT:
                break;
            default:
                break;
        }

        return slots;
    }
    public void HighlightApplicableFieldSlots(bool turnHighlightsOn, Card cardToPlay)
    {
        if (turnHighlightsOn)
        {
            //For every applicable slot on field
            foreach (GameObject slot in ApplicableFieldSlotsToPlay(true, cardToPlay))
            {
                //if the slot doesn't have a monster on it
                if (slot.transform.childCount == 0)
                {
                    slot.GetComponent<Animator>().SetBool("isGlowing", true);
                }
            }
        }
        else
        {
            DisableHighlightsForAllFieldSlots();
        }
    }

    //Disables all highlight animations
    public void DisableHighlightsForAllFieldSlots()
    {
        //For every slot in playermonster
        foreach (GameObject slot in playerMonsterSlots)
        {
            slot.GetComponent<Animator>().SetBool("isGlowing", false);
        }
        //For every slot in enemymonster
        foreach (GameObject slot in playerMonsterSlots)
        {
            slot.GetComponent<Animator>().SetBool("isGlowing", false);
        }
        //FieldSpell card slot
        fieldCardSlot.GetComponent<Animator>().SetBool("isGlowing", false);
    }

    public bool canPlayCardOnThisFieldSlot(Card card, GameObject fieldSlot)
    {
        Card.CARDTYPE cardType = card.cardType;
        //If Card type = Monster
        if (cardType == Card.CARDTYPE.MONSTER)
        {
            //Monster Card Slot is owned by player
            if (playerMonsterSlots.Contains(fieldSlot)) //TODO && turnphase == play state
            {
                //If monster slot doesn't have monster already on it
                if (fieldSlot.transform.childCount == 0)
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }

    public static void SendCardObjectToGraveyard(GameObject cardToRemove)
    {
        //Grab the field Manager script so the card can be assigned to the graveyard
        FieldManager fieldManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<FieldManager>();

        CardDetails cardDetails = cardToRemove.GetComponent<CardDetails>();
        Card cardData = cardDetails.card;


        if (cardData.cardAllegiance == Card.ALLEGIANCE.PLAYER)
        {
            fieldManager.playerGraveyardList.Add(cardToRemove);
            cardToRemove.transform.SetParent(fieldManager.playerGraveyardSlot.transform, false);
        }
        //Place card in horde's graveyard
        if (cardData.cardAllegiance == Card.ALLEGIANCE.HORDE)
        {
            fieldManager.hordeGraveyardList.Add(cardToRemove);
            cardToRemove.transform.SetParent(fieldManager.hordeGraveyardSlot.transform, false);
        }

        //Deactive card from being visible
        cardToRemove.SetActive(false);

        ///PLACES TO ADD THIS METHOD:
        ///player hand full on draw
        ///on card death - done
        ///at end of spell effect
        ///at end of enchantment effect
        ///when horde tries to play monster, but monster field is full

    }


}
