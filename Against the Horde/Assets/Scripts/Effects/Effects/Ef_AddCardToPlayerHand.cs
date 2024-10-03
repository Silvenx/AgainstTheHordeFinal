using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/AddCard_ToPlayerHand_FromNothing")]
public class Ef_AddCardToPlayerHand : Effect
{
    public override IEnumerator ActivateEffect(Target target, GameObject thisCard)
    {
        yield return null;
    }


    ////Card to create and add to player hand
    //public List<CardObjects> cardsToAddToHand;

    //public override void ActivateEffect(Target target, GameObject thisCard = null)
    //{
    //    PlayerManager playerManager = GameManager.Instance.playerManager;

    //    if (cardsToAddToHand != null)
    //    {
    //        for (int i = 0; i < cardsToAddToHand.Count; i++)
    //        {
    //            playerManager.CreateNewCardAndMoveToHand(cardsToAddToHand[i].card);
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError($"Card not assigned to Effect Scriptable Object");
    //    }
    //}

}