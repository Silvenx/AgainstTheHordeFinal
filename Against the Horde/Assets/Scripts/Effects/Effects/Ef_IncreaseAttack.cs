using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Increase_Attack")]
public class Ef_IncreaseAttack : Effect
{
    //Amount to increase attack power by
    public int power = 1;

    public override void ActivateEffect(Target target, GameObject thisCard)
    {
        Debug.Log("Increase Attack of " + target + " by " + power + ".");

        foreach (GameObject o in target.getTargets(thisCard))
        {
            o.GetComponent<CardDetails>().ModifyAttack(+power);
        }
    }
}
