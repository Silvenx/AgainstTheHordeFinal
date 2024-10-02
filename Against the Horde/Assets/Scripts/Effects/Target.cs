using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : ScriptableObject
{
    // Synchronous target selection
    public virtual GameObject[] getTargets(GameObject thisCard = null)
    {
        // Default implementation throws an exception or returns an empty array
        throw new NotImplementedException("Synchronous getTargets() is not implemented. Use GetTargetsAsync() instead.");
        // Alternatively, return new GameObject[0];
    }

    // Asynchronous target selection
    public virtual void GetTargetsAsync(GameObject thisCard, Action<GameObject[]> callback)
    {
        // Default implementation uses synchronous getTargets()
        callback?.Invoke(getTargets(thisCard));
    }

    // Methods needed by TargetManager
    public virtual List<GameObject> GetPotentialTargets()
    {
        return new List<GameObject>();
    }

    public virtual int GetNumberOfTargets()
    {
        return 0;
    }

    // Optional property to indicate if target selection is asynchronous
    public virtual bool RequiresAsyncSelection => false;
}



//public enum TargetType
//{
//    NONE, //used for effects that don't require targets
//    SELF,
//    ALLALLYMONSTERS,
//    ALLENEMYMONSTERS,
//    PLAYERCHARACTER,
//    HORDECHARACTER,
//    //Manual Targeting
//    MANUAL_ADJACENTMONSTER,
//    MANUAL_TARGETMONSTER
//}
