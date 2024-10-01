using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_FieldMonsters")]
public class Tg_FieldMonsters : Target
{
    public FieldToTarget targetGroup;

    public enum FieldToTarget
    {
        ALL_MONSTERS,
        PLAYER_MONSTERS,
        HORDE_MONSTERS
    }

    public override GameObject[] getTargets(GameObject thisCard = null)
    {
        FieldManager fieldManager = GameManager.Instance.fieldManager;

        List<GameObject> targets = new List<GameObject>();

        switch (targetGroup)
        {
            //Adds Player Monsters to List
            case FieldToTarget.PLAYER_MONSTERS:
                targets.AddRange(fieldManager.getAllPlayerMonsters());
                break;

            //Adds Horde Monsters to List
            case FieldToTarget.HORDE_MONSTERS:
                targets.AddRange(fieldManager.getAllHordeMonsters());
                break;

            //ALL MONSTERS ON FIELD
            default:
                targets.AddRange(fieldManager.getAllPlayerMonsters());
                targets.AddRange(fieldManager.getAllHordeMonsters());
                break;
        }

        return targets.ToArray();
    }


}
