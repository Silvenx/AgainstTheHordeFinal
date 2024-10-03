using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_Self")]
public class Tg_Self : Target
{

    public override IEnumerator TargetAquisition(GameObject thisCard = null)
    {
        yield return null;
    }

    public override GameObject[] getTargets()
    {
        return null;
    }

    //public override GameObject[] TargetAquisition(GameObject thisCard)
    //{
    //    return new GameObject[] { thisCard };
    //}
}
