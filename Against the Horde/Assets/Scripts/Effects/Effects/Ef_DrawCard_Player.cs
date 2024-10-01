using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/DrawCard_Player")]
public class Ef_DrawCard_Player : Effect
{
    public int numberOfCards = 1;


    public override void ActivateEffect(Target target, GameObject thisCard = null)
    {
        PlayerManager playerManager = GameManager.Instance.playerManager;

        playerManager.DrawCardFromTopOfDeck(numberOfCards);
    }

}
