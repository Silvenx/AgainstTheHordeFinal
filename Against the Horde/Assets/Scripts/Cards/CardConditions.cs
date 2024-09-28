public enum ConditionType
{
    Tough,
    SpellPower,
    Lifesteal,
    Bleed,
    Poison,
    Regenerate,
    Quick,
    DivineShield,
    Trample,
    Challenge,
    Silence,
    Fragile,
    Cleave
}

[System.Serializable]
public class CardCondition
{
    public ConditionType conditionType; // The type of condition 
    public int value; // The value associated with the condition, stacks essentially

    public CardCondition(ConditionType conditionType, int value)
    {
        this.conditionType = conditionType;
        this.value = value;
    }
}

//JP 28.09.24 - removed trying something new

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardConditions
{
    [Header("Card Conditions")]
    public int tough; //FUTURE: Currently tough is true or false but may change to stack in the future
    public int spellPower; //can stack
    public int lifesteal; //FUTURE: Currently only true or false but may change in the future
    public int bleed;
    public int poison;
    public int regenerate;
    public bool quick;
    public bool divineShield;
    public bool trample;
    public bool challenge;
    public bool silence;
    public bool fragile;
    public bool cleave;
}
*/