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



    public List<GameObject> ApplicableFieldSlotsToPlay(bool isPlayerCard, Card card)
    {
        List<GameObject> slots = new List<GameObject>();
        GameManager.TurnPhase currentTurnPhase = gameManager.currentTurn;

        switch (card.cardType)
        {
            case Card.CARDTYPE.MONSTER:
                //If Horde is playing
                if (!isPlayerCard)
                {
                    //For every slot in hordemonster
                    foreach (GameObject slot in hordeMonsterSlots)
                    {
                        //if the slot doesn't have a monster on it
                        if (slot.transform.childCount == 0)
                        {
                            slots.Add(slot);
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
                break;
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

    public static void SendCardObjectToGraveyard(GameObject cardToRemove, bool isCardAllegiancePlayer)
    {
        FieldManager fieldManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<FieldManager>();

        //Place card in player's graveyard
        if (isCardAllegiancePlayer)
        {
            fieldManager.playerGraveyardList.Add(cardToRemove);
            cardToRemove.transform.SetParent(fieldManager.playerGraveyardSlot.transform, false);
        }
        //Place card in horde's graveyard
        else
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
