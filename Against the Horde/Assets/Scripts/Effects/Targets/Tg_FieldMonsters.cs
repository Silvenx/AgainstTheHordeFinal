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

        finalList.AddRange(targets);
        yield return null;
    }
}
