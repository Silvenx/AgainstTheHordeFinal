using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Condition_Apply")]
public class Ef_ApplyCondition : Effect
{
    public ConditionType conditionToGive;
    public int val = 0;


    //------------------------//

    public override IEnumerator ActivateEffect(Target target, GameObject thisCard)
    {
        // Start the target selection coroutine and wait for it to complete
        yield return GameManager.Instance.StartCoroutine(target.TargetAquisition(thisCard));

        GameObject[] targets = target.getTargets();

        ThisEffect(targets, thisCard);
        
    }

    public override void ThisEffect(GameObject[] targets, GameObject thisCard)
    {
        // Get the selected targets after selection is complete
        if (targets != null)
        {
            foreach (GameObject o in targets)
            {
                CardDetails d = o.GetComponent<CardDetails>();
                if (d != null)
                {
                    d.AddCondition(conditionToGive, val);
                }
            }
        }
        else
        {
            Debug.LogError("No target exists for adding condition: " + conditionToGive);
        }
    }
}
