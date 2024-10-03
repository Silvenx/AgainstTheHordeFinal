using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/AddCard_ToPlayerHand_FromDeck")]
public class Ef_AddCardToPlayerHandFromDeck : Effect
{
    //Card to create and add to player hand
    public List<CardObjects> cardsToDrawFromDeck;

    public override IEnumerator ActivateEffect(Target target, GameObject thisCard)
    {
        yield return null;
    }

    //public override void ActivateEffect(Target target, GameObject thisCard = null)
    //{
    //    PlayerManager playerManager = GameManager.Instance.playerManager;

    //    if (cardsToDrawFromDeck != null)
    //    {
    //        for (int i = 0; i < cardsToDrawFromDeck.Count; i++)
    //        {
    //            playerManager.DrawCardFromDeck(cardsToDrawFromDeck[i].card);
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError($"Card not assigned to Effect Scriptable Object");
    //    }
    //}
}
