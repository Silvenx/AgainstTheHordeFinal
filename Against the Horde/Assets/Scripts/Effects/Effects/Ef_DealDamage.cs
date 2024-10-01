using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Deal Damage")]
public class Ef_DealDamage : Effect
{
    //DamageToDeal
    public int damage = 4;

    public override void ActivateEffect(Target target, GameObject thisCard)
    {
        foreach (GameObject o in target.getTargets())
        {
            o.GetComponent<CardDetails>().ModifyCurrentHealth(-damage);
        }
    }
}
