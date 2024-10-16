using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Multi_Effect")]
public class Ef_MultipleEffects : Effect
{
    // List of effects to do
    public List<Effect> effects;


    //------------------------//

    public override IEnumerator ActivateEffect(Target target, GameObject thisCard)
    {
        // Start the target selection coroutine and wait for it to complete
        yield return GameManager.Instance.StartCoroutine(target.TargetAquisition(thisCard));

        // Get the selected targets after selection is complete
        GameObject[] targets = target.getTargets();

        ThisEffect(targets, thisCard);
    }

    public override void ThisEffect(GameObject[] targets, GameObject thisCard)
    {
        foreach (Effect eff in effects)
        {
            eff.ThisEffect(targets, thisCard);
        }
    }
}
