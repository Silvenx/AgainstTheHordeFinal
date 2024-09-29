using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Triggers/On Draw")]
public class Tr_OnDRAW : Trigger
{
    public override bool Check(GameObject card, EventType eventType)
    {
        return eventType == EventType.OnDRAW;
    }
}

