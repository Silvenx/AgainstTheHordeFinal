using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_EdgeMostFieldMonsters")]
public class Tg_EdgeMostFieldMonster : Target
{
    //FUTURE: Add in horde targeting
    public TargetMonsterField targetSideOfField;
    public WhichEdge targetSide;

    public enum TargetMonsterField
    {
        PLAYER,
        HORDE,
        BOTH
    }
    public enum WhichEdge
    {
        LEFTMOST,
        RIGHTMOST
    }

    public override IEnumerator TargetAquisition(GameObject thisCard = null)
    {
        yield return null;
    }

    public override GameObject[] getTargets()
    {
        return null;
    }


    //public override GameObject[] TargetAquisition(GameObject thisCard = null)
    //{
    //    FieldManager fieldManager = GameManager.Instance.fieldManager;

    //    //Get all active player monsters
    //    List<GameObject> playerMonsterList = fieldManager.getAllPlayerMonsters();
    //    //Get all active horde monsters
    //    List<GameObject> hordeMonsterList = fieldManager.getAllHordeMonsters();

    //    ///List to return
    //    List<GameObject> target = new List<GameObject>();

    //    //Which side of the field
    //    switch (targetSide)
    //    {
    //        //Leftmost
    //        case WhichEdge.LEFTMOST:

    //            switch (targetSideOfField)
    //            {
    //                case TargetMonsterField.PLAYER:
    //                    //Add left most to the list
    //                    target.Add(playerMonsterList[0]);
    //                    break;

    //                case TargetMonsterField.HORDE:
    //                    //Add left most to the list
    //                    target.Add(hordeMonsterList[0]);
    //                    break;

    //                case TargetMonsterField.BOTH:
    //                    //Add left most to the list
    //                    target.Add(playerMonsterList[0]);
    //                    //Add left most to the list
    //                    target.Add(hordeMonsterList[0]);
    //                    break;
    //            }
    //            break;

    //        //Rightmost
    //        case WhichEdge.RIGHTMOST:
    //            switch (targetSideOfField)
    //            {
    //                case TargetMonsterField.PLAYER:
    //                    //Add left most to the list
    //                    target.Add(playerMonsterList[playerMonsterList.Count-1]);
    //                    break;

    //                case TargetMonsterField.HORDE:
    //                    //Add left most to the list
    //                    target.Add(hordeMonsterList[hordeMonsterList.Count - 1]);
    //                    break;

    //                case TargetMonsterField.BOTH:
    //                    //Add left most to the list
    //                    target.Add(playerMonsterList[playerMonsterList.Count - 1]);
    //                    //Add left most to the list
    //                    target.Add(hordeMonsterList[hordeMonsterList.Count - 1]);
    //                    break;
    //            }
    //            break;
    //    }
    //    //Debug.Log("Targetable Player Monsters = "+ playerMonsterList.Count);
    //    //Debug.Log("Targetable Horde Monsters = " + playerMonsterList.Count);
    //    //Debug.Log("Current Target LIST - " + target);
    //    //Debug.Log("Current Target Array - "+ target.ToArray());

    //    return target.ToArray();
    //}

}
