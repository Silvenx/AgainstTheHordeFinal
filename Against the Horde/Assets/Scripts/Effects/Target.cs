using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class Target : ScriptableObject
{
    public abstract GameObject[] getTargets(GameObject thisCard = null);
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
