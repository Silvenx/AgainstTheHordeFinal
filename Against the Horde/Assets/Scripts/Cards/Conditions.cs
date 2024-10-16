public enum ConditionType
{
    Tough,           //Reduces damage taken by X from every unique source of damage
    Spellpower,      //Increases damage done by spell effects (stacks)
    Lifesteal,       //Heals the player for X when the monster attacks
    Bleed,           //Deals X damage to the bleeding monster, lasting X turns. X-1 at end of each round
    Poison,          //Reducing healing received to 0, lasting X turns. x-1 at end of each round
    Regenerate,      //Heals X life to the monster, lasting X turns. X-1 at end of each round
    Quick,           //During Combat, attacks & resolves this creature's damage dealt first.
    DivineShield,    //Completely negates the next damaging effect. Then divine shield vanishes
    Trample,         //If killing an opposing monster, any excess damage is dealt to the opposing character's life points
    Challenge,       //Once per turn, can choose another oppositng monster to be moved in front of this
    Silence,         //While silenced, all effects and conditions no longer activate
    Fragile,         //If monster takes damage or deals damage, it dies
    Cleave,          //During combat, damage is dealt to monsters adjacent to the targeted monster
    Retaliate,       //In combat, when a monster takes damage from another, deal damage equal to the retaliate stacks back
    Burn,            //This monster takes one damage per stack of burning. At the end of the turn remove burn.
    Equip,           //This card's power, life, and effects are applied to the card it's attached to
    Evolve,          //When triggered, this card dies and is replaced with it's evolution
    Stun,            //This monster cannot attack. At the end of the turn reduce the stacks by one
    Doom             //If this monster's current life equals the stacks of doom on it, it dies
}

[System.Serializable]
public class Condition
{
    public ConditionType conditionType; // The type of condition 
    public int value; // The value associated with the condition, stacks essentially
    //public bool isActive;

    public Condition(ConditionType conditionType, int value)
    {
        this.conditionType = conditionType;
        this.value = value;
        //this.isActive = isActive;
    }
}


