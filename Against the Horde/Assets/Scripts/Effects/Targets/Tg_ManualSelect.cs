using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_ManualSelect")]
public class Tg_ManualSelect : Target
{
    public FieldToTarget targetGroup;
    public int numberOfTargets = 1; // default to 1 just in case
    public bool canTargetSelf = true;
    public bool canSelectSameCardMoreThanOnce = false;

    public enum FieldToTarget
    {
        ALL_MONSTERS,
        PLAYER_MONSTERS,
        HORDE_MONSTERS
    }


    //------------------------//

    public override IEnumerator TargetAquisition(GameObject thisCard = null)
    {
        finalList.Clear();
        // Start the coroutine to handle player selection and wait for it to complete
        yield return GameManager.Instance.StartCoroutine(HandlePlayerSelection(thisCard));
    }

    // Method to retrieve selected targets after selection is complete
    public override GameObject[] getTargets()
    {
        return finalList.ToArray();
    }

    //--------------------------------------Targeting Logic--------------------------------------//

    private IEnumerator HandlePlayerSelection(GameObject thisCard)
    {
        GameManager gameManager = GameManager.Instance;
        PlayerManager playerManager = gameManager.playerManager;
        FieldManager fieldManager = gameManager.fieldManager;

        List<GameObject> potentialTargets = new List<GameObject>();

        switch (targetGroup)
        {
            case FieldToTarget.PLAYER_MONSTERS:
                potentialTargets.AddRange(fieldManager.getAllPlayerMonsters());
                break;

            case FieldToTarget.HORDE_MONSTERS:
                potentialTargets.AddRange(fieldManager.getAllHordeMonsters());
                break;

            case FieldToTarget.ALL_MONSTERS:
                potentialTargets.AddRange(fieldManager.getAllPlayerMonsters());
                potentialTargets.AddRange(fieldManager.getAllHordeMonsters());
                break;
        }

        //If not allowed to target self, remove thisCard from list of potential targets
        if (!canTargetSelf)
        {
            //If list of potential targets includes this card
            if (potentialTargets.Contains(thisCard))
            {
                potentialTargets.Remove(thisCard);
            }
        }

        //If no potential targets, end selection method
        if (potentialTargets.Count == 0)
        {
            yield return null;
        }

        // FUTURE: Add in highlight targets here

        while (finalList.Count < numberOfTargets)
        {
            // Wait for player input
            bool haveTargetList = false;

            while (!haveTargetList)
            {
                Debug.Log("No Input");

                //if LMB down
                if (Input.GetMouseButtonDown(0))
                {
                    //Get list of all scene objects that mouse is currently over
                    List<GameObject> objectsUnderMouse = playerManager.objectsUnderMouseOnClickCardObject;
                    //For each object in list...
                    foreach(GameObject card in potentialTargets)
                    {
                        //Check if one of those objects is a card in the potential list of targets
                        if (objectsUnderMouse.Contains(card))
                        {
                            //If I can not select a card more than once & this card has already been selected
                            if (!canSelectSameCardMoreThanOnce && finalList.Contains(card))
                            {
                                Debug.Log("Can not choose this target: " + card.name);
                                //Don't add card to list
                                break;
                            }
                            //Otherwise, good to add card to list
                            else
                            {
                                //Add card to list of selectabled objects
                                finalList.Add(card);
                                Debug.Log("Card " + card.name + " was successfully found.");
                                haveTargetList = true;
                                break;
                            }
                        }
                    }
                }
                //check gamemanager for card mouse is currently over
                //if card is a card on the applicable list of targets
                //add to list of targets and say input received = true

                yield return null;
            }
        }
    }

    
}