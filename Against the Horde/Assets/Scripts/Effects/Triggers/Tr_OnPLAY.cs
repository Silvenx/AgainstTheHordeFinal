using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Triggers/On Play")]
public class Tr_OnPLAY : Trigger
{
    public override bool Check(GameObject card, TriggerType eventType)
    {
        return eventType == TriggerType.PLAY;
    }
}

