using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_Self")]
public class Tg_Self : Target
{
    private GameObject selfCard;

    public override IEnumerator TargetAquisition(GameObject thisCard = null)
    {
        selfCard = thisCard;
        yield return null;
    }

    public override GameObject[] getTargets()
    {
        if (selfCard != null)
        {
            return new GameObject[] { selfCard };
        }

        // Return empty array if no valid card was passed in
        return new GameObject[0];
    }

    //public override GameObject[] TargetAquisition(GameObject thisCard)
    //{
    //    return new GameObject[] { thisCard };
    //}
}
