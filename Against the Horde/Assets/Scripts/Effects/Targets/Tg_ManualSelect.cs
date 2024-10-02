using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_ManualSelect")]
public class Tg_ManualSelect : Target
{
    public FieldToTarget targetGroup;
    public int numberOfTargets = 1; // Default to 1

    public enum FieldToTarget
    {
        ALL_MONSTERS,
        PLAYER_MONSTERS,
        HORDE_MONSTERS
    }

    public override void GetTargetsAsync(GameObject thisCard, Action<GameObject[]> callback)
    {
        // Start the target selection via TargetManager
        TargetManager.Instance.StartTargetSelection(this, thisCard, callback);
    }

    public override List<GameObject> GetPotentialTargets()
    {
        FieldManager fieldManager = GameManager.Instance.fieldManager;
        List<GameObject> potentialTargets = new List<GameObject>();

        switch (targetGroup)
        {
            case FieldToTarget.PLAYER_MONSTERS:
                potentialTargets.AddRange(fieldManager.getAllPlayerMonsters());
                break;
            case FieldToTarget.HORDE_MONSTERS:
                potentialTargets.AddRange(fieldManager.getAllHordeMonsters());
                break;
            case FieldToTarget.ALL_MONSTERS:
                potentialTargets.AddRange(fieldManager.getAllPlayerMonsters());
                potentialTargets.AddRange(fieldManager.getAllHordeMonsters());
                break;
        }
        Debug.Log(potentialTargets);
        return potentialTargets;
    }

    public override int GetNumberOfTargets()
    {
        return numberOfTargets;
    }

    // You can override getTargets() if needed
}
