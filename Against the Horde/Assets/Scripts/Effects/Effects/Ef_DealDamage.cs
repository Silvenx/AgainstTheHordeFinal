using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Deal Damage")]
public class Ef_DealDamage : Effect
{
    //DamageToDeal
    public int damage = 1;


    //------------------------//

    public override IEnumerator ActivateEffect(Target target, GameObject thisCard)
    {
        yield return GameManager.Instance.StartCoroutine(target.TargetAquisition(thisCard));

        GameObject[] targets = target.getTargets();

        ThisEffect(targets, thisCard);
    }

    public override void ThisEffect(GameObject[] targets, GameObject thisCard)
    {
        foreach (GameObject o in targets)
        {
            o.GetComponent<CardDetails>().TakeLifeDamage(damage);
        }
    }
}
