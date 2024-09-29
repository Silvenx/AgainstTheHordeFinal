using UnityEngine;

public abstract class Trigger : ScriptableObject
{
    public abstract bool Check(GameObject card, EventType eventType);
}

public enum EventType
{
    OnDRAW,
    OnPLAY,
    OnDEATH,
    OnATTACK,
    OnKILL,
    OnHORDESPELLCAST,
    OnPLAYERSPELLCAST,
    OnDAMAGED
}
