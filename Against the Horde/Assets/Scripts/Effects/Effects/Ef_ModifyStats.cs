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

        // Get the selected targets after selection is complete
        GameObject[] targets = target.getTargets();
        //foreach(GameObject o in target.getTargets())
        //{
        //    Debug.Log("Targets = "+o);
        //}
        // If list of targets is populated
        if (targets != null || targets.Length == 0)
        {
            //Debug.Log("Card is good to modify stats for: "+targets.Length);
            foreach (GameObject o in targets)
            {
                try
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
                catch (System.Exception) { Debug.LogWarning("Target is not of type Tg_ManualSelect"); }
                //catch (MissingReferenceException) { Debug.LogWarning("Target is not of type Tg_ManualSelect"); }
                //catch (UnassignedReferenceException) { Debug.LogWarning("Target is not of type Tg_ManualSelect"); }
                //catch (System.NullReferenceException) { Debug.LogWarning("Target is not of type Tg_ManualSelect"); }
            }
        }
        else
        {
            Debug.LogError("Target is not of type Tg_ManualSelect");
        }
    }
}
