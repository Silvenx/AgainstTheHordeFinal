using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Empower Adjacent")]
public class Ef_EmpowerAdjacent : Effect
{
    public int power = 1;
    public override void ExecuteEffect(GameObject card, GameObject target)
    {
        //State cardslot and check for null in case
        Transform cardSlot = card.transform.parent;
        if (cardSlot == null)
        {
            Debug.LogError("Oi, Card is not in a slot");
            return;
        }

        //Find the parent and get the list of siblings
        Transform fieldParent = cardSlot.parent;
        int slotIndex = cardSlot.GetSiblingIndex();

        //Make a list called adjacent cards to buff up later
        List<GameObject> adjacentCards = new List<GameObject>();

        //Check for a slot to the left
        if (slotIndex > 0)
        {
            //Checks for whether it's above 0 in the order and takes 1 off current slot to get the left side
            Transform leftSlot = fieldParent.GetChild(slotIndex - 1);
            //Checks for a child under the left slot
            if (leftSlot.childCount > 0)
            {
                //Add to adjacent card list
                adjacentCards.Add(leftSlot.GetChild(0).gameObject);
            }

        }
        //Check for a slot to the right
        if (slotIndex < fieldParent.childCount - 1)
        {
            //Checks for whether there are more cards in the list first then on the right side
            Transform rightSlot = fieldParent.GetChild(slotIndex + 1);
            //Checks for a child under the right slot
            if (rightSlot.childCount > 0)
            {
                //Add to adjacent card list
                adjacentCards.Add(rightSlot.GetChild(0).gameObject);
            }

        }

        foreach (GameObject adjacentCard in adjacentCards)
        {
            CardDetails adjacentCardDetails = adjacentCard.GetComponent<CardDetails>();
            if (adjacentCardDetails != null)
            {
                adjacentCardDetails.ModifyAttack(power);
                Debug.Log($"Increased attack of {adjacentCardDetails.card.cardName} by {power}");
            }
        }

    }
}
