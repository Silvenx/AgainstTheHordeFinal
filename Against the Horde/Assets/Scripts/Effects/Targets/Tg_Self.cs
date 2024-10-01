using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_Self")]
public class Tg_Self : Target
{
    public override GameObject[] getTargets(GameObject thisCard)
    {
        return new GameObject[] { thisCard };
    }
}
