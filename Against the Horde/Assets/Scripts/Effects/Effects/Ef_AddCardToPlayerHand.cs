using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/AddCard_ToPlayerHand_FromNothing")]
public class Ef_AddCardToPlayerHand : Effect
{
    public FromWhere whereDoesCardComeFrom;
    //Card to create and add to player hand
    public List<CardObjects> cardsToAddToHand;

    public enum FromWhere
    {
        THINAIR, //Creates new card from the void
        DECK,
        GRAVEYARD //not set up yet
    }


    //------------------------//

    public override IEnumerator ActivateEffect(Target target, GameObject thisCard)
    {
        ThisEffect();

        yield return null;
    }

    public override void ThisEffect(GameObject[] targets = null, GameObject thisCard = null)
    {
        //If cards have been assigned to this SO
        if (cardsToAddToHand != null)
        {
            PutCardToHand();
        }
        //Cards to "create" has not been assigned to the Scriptable Object
        else
        {
            Debug.LogError($"Card not assigned to Effect Scriptable Object");
        }
    }

    public void PutCardToHand()
    {
        PlayerManager playerManager = GameManager.Instance.playerManager;
        switch (whereDoesCardComeFrom)
        {
            case FromWhere.DECK:
                for (int i = 0; i < cardsToAddToHand.Count; i++)
                {
                    playerManager.DrawCardFromDeck(cardsToAddToHand[i].card);
                }
                break;

            case FromWhere.GRAVEYARD:
                for (int i = 0; i < cardsToAddToHand.Count; i++)
                {
                    //TODO: Create method to remove card from graveyard and put it into hand. Located in playerManager
                }
                break;

            //DEFAULTS TO CREATING CARD FROM VOID/NOTHING/THINAIR
            default:
                for (int i = 0; i < cardsToAddToHand.Count; i++)
                {
                    playerManager.CreateNewCardAndMoveToHand(cardsToAddToHand[i].card);
                }
                break;
        }
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