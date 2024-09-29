using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Draw Card")]
public class Ef_DrawCard : Effect
{
    public int numberOfCards = 1;
    public override void ExecuteEffect(GameObject card, GameObject target)
    {
        //playerManager.DrawCardFromTopOfDeck(numberOfCards);
        throw new System.NotImplementedException();
    }
}
