using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_FacingFieldMonster")]
public class Tg_FacingFieldMonster : Target
{

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
        //Find Card's Position on the Field.
        //bool = isCardonPlayer's Field. int = where on field that card is (0 = left most)
        (bool, int) fieldSlot = fieldManager.getCardsFieldSlotPosition(thisCard);

        List<GameObject> targets = new List<GameObject>();
        try
        {
            targets.Add(fieldManager.getMonsterAt(!fieldSlot.Item1, (fieldSlot.Item2)));
        }
        catch (UnityException e) { Debug.LogWarning("No monster on opposite side of this card. Targeting failed." + e); }


        finalList.AddRange(targets);

        yield return null;
    }

    
}
