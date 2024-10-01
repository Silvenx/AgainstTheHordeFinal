using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_FieldMonsters")]
public class Tg_FieldMonsters : Target
{
    public fieldToTarget targetGroup;

    public enum fieldToTarget
    {
        ALL_MONSTERS,
        PLAYER_MONSTERS,
        HORDE_MONSTERS
    }

    public override GameObject[] getTargets(GameObject thisCard = null)
    {
        FieldManager fieldManager = GameManager.Instance.fieldManager;

        //Target = All monsters on the field
        if (targetGroup == fieldToTarget.PLAYER_MONSTERS)
        {
            List<GameObject> targets = new List<GameObject>();

            targets.AddRange(fieldManager.getAllPlayerMonsters());
            targets.AddRange(fieldManager.getAllHordeMonsters());

            return targets.ToArray();
        }

        //Target = All player monsters on the field
        else if (targetGroup == fieldToTarget.PLAYER_MONSTERS)
        {
            return fieldManager.getAllPlayerMonsters().ToArray();
        }

        //Target = All horde monsters on the field
        else  //targetGroup == fieldToTarget.HORDE_MONSTERS
        {
            return fieldManager.getAllHordeMonsters().ToArray();
        }

    }


}
