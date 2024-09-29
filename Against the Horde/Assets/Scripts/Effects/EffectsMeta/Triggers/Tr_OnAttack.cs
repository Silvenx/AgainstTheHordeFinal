using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Triggers/On Attack")]
public class Tr_OnATTACK : Trigger
{
    public override bool Check(GameObject card, EventType eventType)
    {
        return eventType == EventType.OnATTACK;
    }
}

