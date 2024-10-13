using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    [Header("Card Values")]
    public Sprite borderArt;
    public Sprite characterArt;
    public string cardName;
    public string cardDescription;
    public int baseManaCost;

    [HideInInspector]
    public int currentEnergyCost;
    public int baseHealth;
    [HideInInspector]
    public int maximumHealth;
    [HideInInspector]
    public int currentHealth;
    public int baseAttack;
    [HideInInspector]
    public int currentAttack;

    //Card Type
    public CARDTYPE cardType;
    public CARDRARITY cardRarity;
    public ALLEGIANCE cardAllegiance;
    public enum CARDTYPE
    {
        MONSTER,
        SPELL,
        FIELD,
        EQUIPMENT,
        ENCHANTMENT
    }
    public enum CARDRARITY
    {
        NONE,
        BASIC,
        COMMON,
        RARE,
        EPIC,
        LEGENDARY
    }
    public enum ALLEGIANCE
    {
        PLAYER,
        HORDE
    }

    //Audio
    public AudioClip onPlaySound;
    //TODO: Add other audio


    //Triggers and Effects
    [System.Serializable]
    public class Ability
    {
        public TriggerType trigger;
        public Effect effect;
        public Target target;
    }

    [Header("Card Abilities")]
    public List<Ability> abilities;
    public List<Condition> conditions = new List<Condition>();


    public Card(CardObjects co)
    {
        //Applies base details
        this.cardName = co.card.cardName;
        this.cardDescription = co.card.cardDescription;
        this.baseManaCost = co.card.baseManaCost;
        this.baseHealth = co.card.baseHealth;
        this.baseAttack = co.card.baseAttack;

        //Sets current stats to match base stats
        this.currentEnergyCost = co.card.baseManaCost;
        this.maximumHealth = co.card.baseHealth;
        this.currentHealth = co.card.baseHealth;
        this.currentAttack = co.card.baseAttack;

        this.cardType = co.card.cardType;
        this.cardRarity = co.card.cardRarity;
        this.cardAllegiance = co.card.cardAllegiance;

        this.characterArt = co.card.characterArt;
        this.borderArt = co.card.borderArt;

        this.onPlaySound = co.card.onPlaySound;

        this.abilities = new List<Ability>(co.card.abilities);

        this.conditions = new List<Condition>(co.card.conditions);
    }

    public Card(string cardName, CARDTYPE cardType, CARDRARITY cardRarity, string cardDescription, int manaCost, int health, int attack, List<Condition> initialConditions)
    {
        //Applies base details
        this.cardName = cardName;
        this.cardDescription = cardDescription;
        this.baseManaCost = manaCost;
        this.baseHealth = health;
        this.baseAttack = attack;

        //Sets current stats to match base stats
        this.currentEnergyCost = manaCost;
        this.maximumHealth = health;
        this.currentHealth = health;
        this.currentAttack = attack;

        this.cardType = cardType;
        this.cardRarity = cardRarity;

        this.conditions = initialConditions ?? new List<Condition>();

        this.abilities = abilities != null ? new List<Ability>(abilities) : new List<Ability>();

    }
    public Card(string cardName, CARDTYPE cardType, string cardDescription, int manaCost, int health, int attack, int currentManaCost, int currentHealth, int currentAttack)
    {
        //Applies base details
        this.cardName = cardName;
        this.cardDescription = cardDescription;
        this.baseManaCost = manaCost;
        this.baseHealth = health;
        this.baseAttack = attack;

        //Sets current stats as supplied
        this.currentEnergyCost = currentManaCost;
        this.currentHealth = currentHealth;
        this.maximumHealth = currentHealth;
        this.currentAttack = currentAttack;

        this.cardType = cardType;
    }
}
