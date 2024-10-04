using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_AdjacentFieldSlots")]
public class Tg_Adjacent : Target
{
    public SlotsToTarget targetGroup;
    public TargetType targetType;

    public enum SlotsToTarget
    {
        LEFT_AND_RIGHT,
        LEFT,
        RIGHT
    }
    public enum TargetType
    {
        FIELDSLOT,
        MONSTERSONFIELDSLOT
    }

    //------------------------//

    public override IEnumerator TargetAquisition(GameObject thisCard = null)
    {
        finalList.Clear();
        // Start the coroutine to handle player selection and wait for it to complete
        yield return GameManager.Instance.StartCoroutine(GetMyTargets(thisCard));
    }

    // Method to retrieve selected targets after selection is complete
    public override GameObject[] getTargets()
    {
        return finalList.ToArray();
    }

    //--------------------------------------Targeting Logic--------------------------------------//

    public IEnumerator GetMyTargets(GameObject thisCard = null)
    {
        FieldManager fieldManager = GameManager.Instance.fieldManager;
        //Find Card's Position on the Field
        (bool isOnPlayerField, int fieldSlotPos) fieldSlot = fieldManager.getCardsFieldSlotPosition(thisCard);
        int fieldSlotCount = fieldManager.playerMonsterSlots.Count;

        //Debug.Log("IsPlayerField = " + fieldSlot.Item1 + ". FieldSlotPosition = " + fieldSlot.Item2);

        List<GameObject> targets = new List<GameObject>();

        switch (targetGroup)
        {
            //Targets Monster LEFT of current monster
            case SlotsToTarget.LEFT:
                //If placed monster is currently is not left-most slot
                if (fieldSlot.fieldSlotPos != 0)
                {
                    //Do I fetch the field slot GameObject OR monster card GameObject on that field slot
                    switch (targetType)
                    {
                        case TargetType.FIELDSLOT:
                            targets.Add(fieldManager.getFieldSlotAt(fieldSlot.isOnPlayerField, (fieldSlot.fieldSlotPos-1)));
                            break;
                        case TargetType.MONSTERSONFIELDSLOT:
                            targets.Add(fieldManager.getMonsterAt(fieldSlot.isOnPlayerField, (fieldSlot.fieldSlotPos - 1)));
                            break;
                    }
                }
                break;

            //Targets Monster RIGHT of current monster
            case SlotsToTarget.RIGHT:
                //If placed monster is currently is not left-most slot
                if (fieldSlot.fieldSlotPos != fieldSlotCount)
                {
                    //Do I fetch the field slot GameObject OR monster card GameObject on that field slot
                    switch (targetType)
                    {
                        case TargetType.FIELDSLOT:
                            targets.Add(fieldManager.getFieldSlotAt(fieldSlot.isOnPlayerField, (fieldSlot.fieldSlotPos + 1)));
                            break;
                        case TargetType.MONSTERSONFIELDSLOT:
                            targets.Add(fieldManager.getMonsterAt(fieldSlot.isOnPlayerField, (fieldSlot.fieldSlotPos + 1)));
                            break;
                    }
                }
                break;


            //Targets both LEFT and RIGHT monsters of current monster
            default:
                //If placed monster is currently is not left-most slot
                if (fieldSlot.fieldSlotPos != 0)
                {
                    //Do I fetch the field slot GameObject OR monster card GameObject on that field slot
                    switch (targetType)
                    {
                        case TargetType.FIELDSLOT:
                            targets.Add(fieldManager.getFieldSlotAt(fieldSlot.isOnPlayerField, (fieldSlot.fieldSlotPos - 1)));
                            break;
                        case TargetType.MONSTERSONFIELDSLOT:
                            targets.Add(fieldManager.getMonsterAt(fieldSlot.isOnPlayerField, (fieldSlot.fieldSlotPos - 1)));
                            break;
                    }
                }
                //If placed monster is currently is not left-most slot
                if (fieldSlot.fieldSlotPos != fieldSlotCount)
                {
                    //Do I fetch the field slot GameObject OR monster card GameObject on that field slot
                    switch (targetType)
                    {
                        case TargetType.FIELDSLOT:
                            targets.Add(fieldManager.getFieldSlotAt(fieldSlot.isOnPlayerField, (fieldSlot.fieldSlotPos + 1)));
                            break;
                        case TargetType.MONSTERSONFIELDSLOT:
                            targets.Add(fieldManager.getMonsterAt(fieldSlot.isOnPlayerField, (fieldSlot.fieldSlotPos + 1)));
                            break;
                    }
                }
                break;
        }

        finalList.AddRange(targets);

        yield return null;
    }

}
