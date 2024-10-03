using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/AddCard_ToPlayerField")]
public class Ef_AddCardToPlayerField : Effect
{
    // The card to summon (can be set dynamically in the ScriptableObject, e.g., Elite Guard)
    public CardObjects cardToSummon;

    public override IEnumerator ActivateEffect(Target target, GameObject thisCard)
    {
        PlayerManager playerManager = GameManager.Instance.playerManager;
        FieldManager fieldManager = GameManager.Instance.fieldManager;

        // Check if card to summon is set in the ScriptableObject
        if (cardToSummon != null)
        {
            // Find available slots on the player's field
            List<GameObject> availableSlots = fieldManager.ApplicableFieldSlotsToPlay(true, cardToSummon.card);

            if (availableSlots.Count > 0)
            {
                // Instantiate the card object
                GameObject cardObject = GameManager.Instance.CreateCardObject(cardToSummon.card);

                // Place the card in the first available slot
                cardObject.GetComponent<CardDetails>().PlayThisCardOnFieldSlot(availableSlots[0]);

                Debug.Log($"{cardToSummon.card.cardName} summoned and placed in slot {availableSlots[0].name}");
            }
            else
            {
                Debug.LogError("No available slots on the player's field.");
            }
        }
        else
        {
            Debug.LogError("Card to summon is not assigned in the effect ScriptableObject.");
        }

        yield return null;
    }
}