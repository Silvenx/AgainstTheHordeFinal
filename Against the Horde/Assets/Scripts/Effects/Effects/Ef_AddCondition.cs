using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Add Condition")]
public class Ef_AddCondition : Effect
{
    //Amount to increase attack power by
    public int divineShield;

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
                    d.AddCondition(ConditionType.DivineShield, divineShield, true);
                }
            }
        }
        else
        {
            Debug.LogError("Target is not of type Tg_ManualSelect");
        }
    }
}
