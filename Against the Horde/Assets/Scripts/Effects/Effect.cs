using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    //For effects with no target
    public abstract void ActivateEffect(Target targets, GameObject thisCard);
}
