using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Deal Damage")]
public class Ef_DealDamage : Effect
{
    public int damage = 4;
    public override void ExecuteEffect(GameObject card, GameObject target)
    {
        if (target != null)
        {
            target.GetComponent<CardDetails>().ModifyCurrentHealth(-damage);
        }
    }
}
