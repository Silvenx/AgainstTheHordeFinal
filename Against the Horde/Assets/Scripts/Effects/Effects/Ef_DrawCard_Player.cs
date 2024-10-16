using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/DrawCard_Player")]
public class Ef_DrawCard_Player : Effect
{
    //Number of Cards to draw from top of deck
    public int cardsToDraw = 1;


    //------------------------//

    public override IEnumerator ActivateEffect(Target target = null, GameObject thisCard = null)
    {
        ThisEffect();

        yield return null;
    }

    public override void ThisEffect(GameObject[] targets = null, GameObject thisCard = null)
    {
        //Manager
        PlayerManager playerManager = GameManager.Instance.playerManager;

        //Draw a card
        playerManager.DrawCardFromTopOfDeck(cardsToDraw);
    }
}
