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

    


    public List<GameObject> ApplicableFieldSlotsToPlay(Card card)
    {
        List<GameObject> slots = new List<GameObject>();
        GameManager.TurnPhase currentTurnPhase = gameManager.currentTurn;

        //if card is a monster
        if (card.cardType == Card.CARDTYPE.MONSTER)
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
        else if (card.cardType == Card.CARDTYPE.SPELL)
        {

        }
        else if (card.cardType == Card.CARDTYPE.FIELD)
        {

        }
        else if (card.cardType == Card.CARDTYPE.EQUIPMENT)
        {

        }
        else if (card.cardType == Card.CARDTYPE.ENCHANTMENT)
        {

        }

        return slots;
    }

    public void HighlightApplicableFieldSlots(bool turnHighlightsOn, Card cardToPlay)
    {
        if (turnHighlightsOn)
        {
            //For every applicable slot on field
            foreach (GameObject slot in ApplicableFieldSlotsToPlay(cardToPlay))
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

    public static void RemoveCardFromField(GameObject cardToRemove)
    {
        //move to graveyard, blah bah.
    }


}
