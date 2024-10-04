using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/AddCard_ToField")]
public class Ef_AddCardToField : Effect
{
    // The card to summon (can be set dynamically in the ScriptableObject, e.g., Elite Guard)
    public CardObjects cardToSummon;


    //------------------------//

    public override IEnumerator ActivateEffect(Target target, GameObject thisCard)
    {
        // Start the target selection coroutine and wait for it to complete
        yield return GameManager.Instance.StartCoroutine(target.TargetAquisition(thisCard));

        // Get the selected targets after selection is complete
        GameObject[] targets = target.getTargets();

        // Check if card to summon is set in the ScriptableObject
        if (cardToSummon != null)
        {
            // Find available slots on the player's field
            foreach(GameObject slot in targets)
            {
                //Debug.Log("Slot Position = " + slot.name);
                //If slot doesn't have a monster already there...
                if (slot.transform.childCount == 0)
                {
                    //Create Card Object
                    GameObject cardObject = GameManager.Instance.CreateCardObject(cardToSummon.card);

                    // Place the card in the first available slot
                    cardObject.GetComponent<CardDetails>().PlayThisCardOnFieldSlot(slot);
                }
                else
                {
                    Debug.LogWarning("Slot: '"+slot.name+"' is already taken by another monster.");
                }
            }
            
        }
        else
        {
            Debug.LogError("Card to summon is not assigned in the effect ScriptableObject.");
        }

    }
}