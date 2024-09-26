using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    [Header("Card Values")]
    public Sprite cardImage;
    public string cardName;
    public string cardDescription;
    public int baseManaCost;
    [HideInInspector]
    public int currentManaCost;
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
    //public List<EffectTriggers> cardEffects;

    //Audio
    public AudioClip onPlaySound;
    //TODO: Add other audio

    public enum CARDTYPE
    {
        MONSTER,
        SPELL,
        FIELD,
        EQUIPMENT,
        ENCHANTMENT
    }

    public Card(CardObjects co)
    {
        //Applies base details
        this.cardName = co.card.cardName;
        this.cardDescription = co.card.cardDescription;
        this.baseManaCost = co.card.baseManaCost;
        this.baseHealth = co.card.baseHealth;
        this.baseAttack = co.card.baseAttack;

        //Sets current stats to match base stats
        this.currentManaCost = co.card.baseManaCost;
        this.maximumHealth = co.card.baseHealth;
        this.currentHealth = co.card.baseHealth;
        this.currentAttack = co.card.baseAttack;

        this.cardType = co.card.cardType;

        this.cardImage = co.card.cardImage;

        this.onPlaySound = co.card.onPlaySound;
    }

    public Card(string cardName, CARDTYPE cardType, string cardDescription, int manaCost, int health, int attack)
    {
        //Applies base details
        this.cardName = cardName;
        this.cardDescription = cardDescription;
        this.baseManaCost = manaCost;
        this.baseHealth = health;
        this.baseAttack = attack;

        //Sets current stats to match base stats
        this.currentManaCost = manaCost;
        this.maximumHealth = health;
        this.currentHealth = health;
        this.currentAttack = attack;

        this.cardType = cardType;
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
        this.currentManaCost = currentManaCost;
        this.currentHealth = currentHealth;
        this.maximumHealth = currentHealth;
        this.currentAttack = currentAttack;

        this.cardType = cardType;
    }
}
