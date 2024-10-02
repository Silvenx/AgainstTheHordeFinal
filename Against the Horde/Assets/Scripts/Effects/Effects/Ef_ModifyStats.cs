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
        GameObject[] t = target.getTargets(thisCard);
        Debug.Log(t);
        //If there are actual targets available
        try
        {
            foreach (GameObject o in t)
            {
                CardDetails d = o.GetComponent<CardDetails>();
                d.ModifyAttack(+power);
                d.ModifyManaCost(+energy);
                d.ModifyMaximumHealth(+health);
                d.ModifyCurrentHealth(+health);
            }
        }
        catch (System.NullReferenceException e) { Debug.Log("Lacking Target " + e); }
    }
}
