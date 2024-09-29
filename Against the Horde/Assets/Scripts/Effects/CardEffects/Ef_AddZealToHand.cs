using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Add Zeal to Hand")]
public class Ef_AddZealToHand : Effect
{
    public override void ExecuteEffect(GameObject card, GameObject target)
    {
        PlayerManager playerManager = GameManager.Instance.playerManager;

        CardDatabase cardDatabase = GameManager.Instance.cardDatabase;

        Card zealCard = cardDatabase.GetCardByName("Zeal");

        if (zealCard != null)
        {
            playerManager.DrawCardToHand(zealCard);
            Debug.Log("Added 'Zeal' to player's hand.");
        }
        else
        {
            Debug.LogError("Zeal card not found in CardDatabase.");
        }
    }
}