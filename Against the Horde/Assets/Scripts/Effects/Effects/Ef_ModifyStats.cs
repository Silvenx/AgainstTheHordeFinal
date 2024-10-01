using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Increase_Stats")]
public class Ef_ModifyStats : Effect
{
    //Amount to increase attack power by
    public int energy = 0;
    public int power = 0;
    public int health = 0;

    public override void ActivateEffect(Target target, GameObject thisCard)
    {
        Debug.Log("Increase Attack of " + target + " by " + power + ".");

        foreach (GameObject o in target.getTargets(thisCard))
        {
            CardDetails d = o.GetComponent<CardDetails>();
            d.ModifyAttack(+power);
            d.ModifyManaCost(+energy);
            d.ModifyCurrentHealth(+health);
            d.ModifyMaximumHealth(+health);
        }
    }
}
