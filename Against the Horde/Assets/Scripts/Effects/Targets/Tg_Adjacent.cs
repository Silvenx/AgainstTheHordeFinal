using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_AdjacentFieldMonsters")]
public class Tg_Adjacent : Target
{
    public MonstersToTarget targetGroup;
    public enum MonstersToTarget
    {
        LEFT_AND_RIGHT,
        LEFT,
        RIGHT
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
        (bool, int) fieldSlot = fieldManager.getCardsFieldSlotPosition(thisCard);
        int fieldSlotCount = fieldManager.playerMonsterSlots.Count;

        //Debug.Log("IsPlayerField = " + fieldSlot.Item1 + ". FieldSlotPosition = " + fieldSlot.Item2);

        List<GameObject> targets = new List<GameObject>();

        switch (targetGroup)
        {
            //Targets Monster LEFT of current monster
            case MonstersToTarget.LEFT:
                //If monster is currently is not left-most slot
                if (fieldSlot.Item2 != 0)
                {
                    targets.Add(fieldManager.getMonsterAt(fieldSlot.Item1, (fieldSlot.Item2 - 1)));
                }
                break;

            //Targets Monster RIGHT of current monster
            case MonstersToTarget.RIGHT:
                //If monster is currently is not left-most slot
                if (fieldSlot.Item2 != fieldSlotCount)
                {
                    targets.Add(fieldManager.getMonsterAt(fieldSlot.Item1, (fieldSlot.Item2 + 1)));
                }
                break;


            //Targets both LEFT and RIGHT monsters of current monster
            default:
                //If monster is currently is not left-most slot
                if (fieldSlot.Item2 != 0)
                {
                    targets.Add(fieldManager.getMonsterAt(fieldSlot.Item1, fieldSlot.Item2 - 1));
                }
                //If monster is currently is not left-most slot
                if (fieldSlot.Item2 != fieldSlotCount)
                {
                    targets.Add(fieldManager.getMonsterAt(fieldSlot.Item1, fieldSlot.Item2 + 1));
                }
                break;
        }

        finalList.AddRange(targets);

        yield return null;
    }

}
