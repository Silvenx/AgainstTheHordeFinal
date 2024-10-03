using System.Collections;
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

    public override IEnumerator ActivateEffect(Target target, GameObject thisCard)
    {
        // Start the target selection coroutine and wait for it to complete
        yield return GameManager.Instance.StartCoroutine(target.TargetAquisition(thisCard));

        GameObject[] targets = target.getTargets();
        // Get the selected targets after selection is complete
        if (targets != null)
        {
            foreach (GameObject o in targets)
            {
                CardDetails d = o.GetComponent<CardDetails>();
                if (d != null)
                {
                    d.ModifyAttack(power);
                    d.ModifyManaCost(energy);
                    d.ModifyMaximumHealth(health);
                    d.ModifyCurrentHealth(health);
                }
            }
        }
        else
        {
            Debug.LogError("Target is not of type Tg_ManualSelect");
        }
    }
}
