using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    //A Short description of what this effect does (added to the Scriptable Object)
    public string effectDescription;
    public string effDesc { get => effectDescription; }

    //For effects with no target
    public abstract IEnumerator ActivateEffect(Target targets, GameObject thisCard);

    public abstract void ThisEffect(GameObject[] targets, GameObject thisCard);
}
