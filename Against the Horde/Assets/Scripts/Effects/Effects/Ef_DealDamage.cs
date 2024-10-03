using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Deal Damage")]
public class Ef_DealDamage : Effect
{
    //DamageToDeal
    public int damage = 4;

    public override IEnumerator ActivateEffect(Target target, GameObject thisCard)
    {
        yield return null;
    }

    //public override void ActivateEffect(Target target, GameObject thisCard)
    //{
    //    foreach (GameObject o in target.TargetAquisition())
    //    {
    //        o.GetComponent<CardDetails>().TakeLifeDamage(damage);
    //    }
    //}
}
