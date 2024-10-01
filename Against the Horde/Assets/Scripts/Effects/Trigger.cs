using UnityEngine;

public abstract class Trigger : ScriptableObject
{
    public abstract bool Check(GameObject card, TriggerType triggerType);
}

public enum TriggerType
{
    DRAW,
    PLAY,
    DEATH,
    ATTACK,
    KILL,
    ENEMYSPELLCAST,
    MYSPELLCAST,
    DAMAGED
}
