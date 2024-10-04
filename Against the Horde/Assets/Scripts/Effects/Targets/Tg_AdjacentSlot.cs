using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_AdjacentSlots")]
public class Tg_AdjacentSlot : Target
{
    public SlotsToTarget slotsToTarget; // Option to target left, right, or both
    private GameObject selfCard; // Store the reference to the card


    public enum SlotsToTarget
    {
        LEFT_AND_RIGHT,
        LEFT,
        RIGHT
    }

    public override IEnumerator TargetAquisition(GameObject thisCard = null)
    {
        selfCard = thisCard;
        yield return null;
    }

    public override GameObject[] getTargets()
    {

        FieldManager fieldManager = GameManager.Instance.fieldManager;

        // Find the card's position on the field (assumes a function like this exists)
        (bool isPlayerField, int fieldSlotIndex) = fieldManager.getCardsFieldSlotPosition(selfCard);
        int totalFieldSlots = fieldManager.playerMonsterSlots.Count; // Assuming you're targeting player slots

        // List to store the adjacent slots
        List<GameObject> targets = new List<GameObject>();

        switch (slotsToTarget)
        {
            case SlotsToTarget.LEFT_AND_RIGHT:
                // Check and add the left slot
                if (fieldSlotIndex > 0)
                {
                    GameObject leftSlot = fieldManager.getMonsterAt(isPlayerField, fieldSlotIndex - 1);
                    if (leftSlot != null)
                    {
                        targets.Add(leftSlot);
                    }
                }

                // Check and add the right slot
                if (fieldSlotIndex < totalFieldSlots - 1)
                {
                    GameObject rightSlot = fieldManager.getMonsterAt(isPlayerField, fieldSlotIndex + 1);
                    if (rightSlot != null)
                    {
                        targets.Add(rightSlot);
                    }
                }
                break;

            case SlotsToTarget.LEFT:
                // Check and add the left slot only
                if (fieldSlotIndex > 0)
                {
                    GameObject leftSlot = fieldManager.getMonsterAt(isPlayerField, fieldSlotIndex - 1);
                    if (leftSlot != null)
                    {
                        targets.Add(leftSlot);
                    }
                }
                break;

            case SlotsToTarget.RIGHT:
                // Check and add the right slot only
                if (fieldSlotIndex < totalFieldSlots - 1)
                {
                    GameObject rightSlot = fieldManager.getMonsterAt(isPlayerField, fieldSlotIndex + 1);
                    if (rightSlot != null)
                    {
                        targets.Add(rightSlot);
                    }
                }
                break;
        }

        // Return the targets as an array
        return targets.ToArray();
    }
}
