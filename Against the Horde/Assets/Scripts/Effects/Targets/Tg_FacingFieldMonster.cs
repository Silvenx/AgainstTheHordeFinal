using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_FacingFieldMonster")]
public class Tg_FacingFieldMonster : Target
{
    public override GameObject[] getTargets(GameObject thisCard = null)
    {
        FieldManager fieldManager = GameManager.Instance.fieldManager;
        //Find Card's Position on the Field.
        //Bool = isCardonPlayer's Field
        (bool, int) fieldSlot = fieldManager.getCardsFieldSlotPosition(thisCard);

        List<GameObject> targets = new List<GameObject>();
        try
        {
            targets.Add(fieldManager.getMonsterAt(!fieldSlot.Item1, (fieldSlot.Item2)));
        }
        catch (UnityException e) { Debug.LogWarning("No monster on opposite side of this card. Targeting failed." + e); }

        

        return targets.ToArray();
    }
}
