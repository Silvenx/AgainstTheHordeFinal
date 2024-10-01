using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Triggers/On Attack")]
public class Tr_OnATTACK : Trigger
{
    public override bool Check(GameObject card, TriggerType eventType)
    {
        return eventType == TriggerType.ATTACK;
    }
}

