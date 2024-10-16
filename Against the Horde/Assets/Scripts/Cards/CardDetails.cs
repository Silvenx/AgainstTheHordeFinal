using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class CardDetails : MonoBehaviour
{

    [Header("Card Values")]
    public Card card;

    [Header("UI Fields")]
    public Image cardBorderImage;
    public Image cardCharacterArtImage;
    public Image divineShieldArt;
    public Image toughArt;
    public Image rarityB;
    public Image rarityC;
    public Image rarityR;
    public Image rarityE;
    public Image rarityL;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardDescriptionText;
    public TextMeshProUGUI manaCostText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;

    public bool canDrag = true;
    [HideInInspector]
    public IEnumerator currentCoroutine;

    //---------------------Card Updates---------------------

    //Forcefully set all card stats to the supplied Card object's details
    public void SetCardDetails(Card cardDetails)
    {
        //Replace's card's base details with supplied details
        this.card.cardName = cardDetails.cardName;
        this.card.cardDescription = cardDetails.cardDescription;
        this.card.baseManaCost = cardDetails.baseManaCost;
        this.card.baseHealth = cardDetails.baseHealth;
        this.card.baseAttack = cardDetails.baseAttack;
        //Sets current stats to match base stats
        this.card.currentEnergyCost = card.baseManaCost;
        this.card.currentHealth = card.baseHealth;
        this.card.maximumHealth = card.baseHealth;
        this.card.currentAttack = card.baseAttack;

        this.card.cardType = cardDetails.cardType;
        this.card.cardRarity = cardDetails.cardRarity;

        this.card.borderArt = cardDetails.borderArt;
        this.card.characterArt = cardDetails.characterArt;

        this.card.cardAllegiance = cardDetails.cardAllegiance;

        this.card.conditions = cardDetails.conditions;

        this.card.abilities = cardDetails.abilities;

        //Applies new card details to UI of this card gameobject
        SetCardUI(card.borderArt, card.characterArt, card.cardName, card.cardDescription, card.baseManaCost, card.baseHealth, card.baseAttack, card.cardRarity);
    }

    //Modifies base stats
    public void SetBaseStats(int manaCost, int health, int attack)
    {
        this.card.baseManaCost = manaCost;
        this.card.baseHealth = health;
        this.card.baseAttack = attack;

        //Updates UI text
        UpdateCardUI();
    }
    public void SetBaseManaCost(int manaCost)
    {
        this.card.baseManaCost = manaCost;

        //Updates UI text
        UpdateCardUI();
    }
    public void SetBaseHealth(int health)
    {
        this.card.baseHealth = health;

        //Updates UI text
        UpdateCardUI();
    }
    public void SetBaseAttack(int attack)
    {
        this.card.baseAttack = attack;

        //Updates UI text
        UpdateCardUI();
    }
    //Modifies current stats
    public void SetCurrentStats(int manaCost, int health, int attack)
    {
        this.card.currentEnergyCost = manaCost;
        this.card.currentHealth = health;
        this.card.currentAttack = attack;

        //Updates UI text
        UpdateCardUI();
    }
    public void SetCurrentManaCost(int manaCost)
    {
        this.card.currentEnergyCost = manaCost;

        //Updates UI text
        UpdateCardUI();
    }
    public void SetCurrentHealth(int health)
    {
        this.card.currentHealth = health;

        //Updates UI text
        UpdateCardUI();
    }
    public void SetCurrentAttack(int attack)
    {
        this.card.currentAttack = attack;

        //Updates UI text
        UpdateCardUI();
    }


    //Retreieves all card details
    public Card GetCardDetails()
    {
        return new Card(card.cardName, card.cardType, card.cardRarity, card.cardDescription, card.baseManaCost, card.baseHealth, card.baseAttack, card.conditions);
    }


    public void ResetCardStatsToBase()
    {
        this.card.currentEnergyCost = card.baseManaCost;
        this.card.currentHealth = card.baseHealth;
        this.card.currentAttack = card.baseAttack;

        //Updates UI text
        UpdateCardUI();
    }

    //--------------------------------------------------- Card Functions ---------------------------------------------------//

    //Play Card on Field Slot
    public void PlayThisCardOnFieldSlot(GameObject fieldSlot)
    {
        RectTransform rect = GetComponent<RectTransform>();

        //Set parent to be the Field Slot
        transform.SetParent(fieldSlot.GetComponent<RectTransform>());

        //Set position to be same as field slot it is placed under
        rect.anchoredPosition = Vector2.zero;

        this.ActivateCardEffect(TriggerType.PLAY);
        UpdateCardUI();
        //currentCoroutine = CharacterManager.LerpObjectMovement(this.gameObject, rect.anchoredPosition, Vector2.zero, 2f, 1.5f);
        //StartCoroutine(currentCoroutine);
    }

    public void PlaySpellCard()
    {
        this.ActivateCardEffect(TriggerType.PLAY);
    }


    //Increase or Decreases the Mana Cost of the card
    public void ModifyManaCost(int amountToIncrease)
    {
        card.currentEnergyCost += amountToIncrease;
        //Check if mana cost below 0
        if (card.currentEnergyCost <= 0)
        {
            card.currentEnergyCost = 0;
        }
        UpdateCardUI();
    }

    //Increase or Decreases the Current Health of the card
    public void ModifyCurrentHealth(int amountToIncrease)
    {
        card.currentHealth += amountToIncrease;

        //Check if dead
        //FUTURE: Would need to put on kill trigger here later but only on field
        //Probably need to prevent cards discarding in hand at 0 life
        if (card.currentHealth <= 0)
        {
            CardDeath();
            return;
        }
        else if (card.currentHealth >= card.maximumHealth)
        {
            card.currentHealth = card.maximumHealth;
        }
        UpdateCardUI();
    }

    public void TakeLifeDamage(int damage)
    {
        bool hasDivineShield = HasCondition(ConditionType.DivineShield);
        bool hasTough = HasCondition(ConditionType.Tough);
        //Divine shield if statement
        //int divineShield = GetConditionValue(ConditionType.DivineShield);
        //int toughReduction = GetConditionValue(ConditionType.Tough);

        if (hasDivineShield)
        {
            Debug.Log("Divine shield blocks the damage");
            damage = 0;
            RemoveCondition(ConditionType.DivineShield);
            UpdateCardUI();
            return;
        }
        else if (hasTough)
        {
            int toughVal = GetConditionValue(ConditionType.Tough);
            //May need to remove or leave this in future depending on whether we need tough to stack
            Debug.Log($"Reducing damage by {toughVal} due to Tough.");
            damage -= toughVal;
        }

        ModifyCurrentHealth(-damage);
    }

    public void TakeLifeTrueDamage(int damage)
    {

    }

    public void HealLife(int healing)
    {
        ModifyCurrentHealth(healing);
    }


    //Increase or Decreases the Maximum Health of the card
    public void ModifyMaximumHealth(int amountToIncrease)
    {
        card.maximumHealth += amountToIncrease;
        //Check if dead
        if (card.maximumHealth <= 0)
        {
            CardDeath();
            return;
        }
        UpdateCardUI();
    }
    //Increase or Decreases the Attack of the card
    public void ModifyAttack(int amountToIncrease)
    {
        card.currentAttack += amountToIncrease;
        //Check if mana cost below 0
        if (card.currentAttack <= 0)
        {
            card.currentAttack = 0;
        }
        UpdateCardUI();
    }

    public void CardDeath()
    {
        //Check if the card is player
        bool isCardAllegiancePlayer = card.cardAllegiance == Card.ALLEGIANCE.PLAYER;

        //FUTURE: Trigger special on-death affects should a card have them (might have to be earlier though)
        //this.gameObject.GetComponent<EffectTriggers>().OnDeath();

        //remove from field
        FieldManager.SendCardObjectToGraveyard(this.gameObject);

        ResetCardStatsToBase();
    }
    //-------------------------------------------- Card Condition Functions --------------------------------------------//


    public void AddCondition(ConditionType conditionType, int value)
    //Add a condition to a card
    {
        //Debug fix value to 1
        if (value <= 0)
        {
            Debug.LogWarning("Value for updating existing condition was " + value + ", which is below 1. Updating to 1.");
            value = 1;
        }

        //Check if condition is already existing and replace/update values accordingly
        foreach (Condition c in card.conditions)
        {
            //If that condition already exists...
            if (c.conditionType == conditionType)
            {
                //Update condition's value
                c.value += value;
                return;
            }
        }
        //If condition doesn't exist on this card...
        //Create new condition and add it to list of conditions
        card.conditions.Add(new Condition(conditionType, value));
        UpdateCardUI();
        Debug.Log($"{conditionType} added with value {value} to {card.cardName}");

        //TODO: switch case for every condition type, applying values/bool as appropriate.
        //& cleaning up list incase duplicates exist as well as incorrect values for conditions
    }

    public void RemoveCondition(ConditionType conditionType)
    //remove a condition completely from a card
    {
        card.conditions.RemoveAll(c => c.conditionType == conditionType);
        UpdateCardUI();
        Debug.Log($"{conditionType} removed from {card.cardName}");
    }

    public bool HasCondition(ConditionType conditionType)
    //Check if a card has a condition
    {
        return card.conditions.Exists(c => c.conditionType == conditionType);
    }

    public int GetConditionValue(ConditionType conditionType)
    //Check if a card has a condition
    {
        Condition condition = card.conditions.Find(c => c.conditionType == conditionType);
        return condition != null ? condition.value : 0; // Return 0 if condition doesn't exist
    }

    public void ModifyConditionValue(ConditionType conditionType, int newValue)
    {
        if (newValue < 0)
        {
            newValue = 0;
        }

        Condition condition = card.conditions.Find(c => c.conditionType == conditionType);
        if (condition != null)
        {
            condition.value = newValue;
            Debug.Log($"{conditionType} modified to {newValue} on {card.cardName}");
        }
        else
        {
            Debug.LogWarning($"{conditionType} not found on {card.cardName}");
        }
        UpdateCardUI();
    }

    //--------------------------------------------------- Card Effects ---------------------------------------------------//

    public void ActivateCardEffect(TriggerType eventType)
    {
        //Exit if card has no abilities
        if (card.abilities == null) { return; }

        //Start the coroutine to handle the ability order and discard
        StartCoroutine(ProcessCardEffect(eventType));
    }

    private IEnumerator ProcessCardEffect(TriggerType eventType)
    {
        //This method handles ability order and discard

        //Go through every ability listed on this card
        for (int i = 0; i < card.abilities.Count; i++)
        {
            var ability = card.abilities[i];

            //If supplied triggerType is the same as the trigger type for this ability
            if (ability.trigger == eventType)
            {
                //Trigger the ability
                yield return StartCoroutine(ability.effect.ActivateEffect(ability.target, this.gameObject));
            }
        }

        //After going through all the abilities, if it's a spell discard it
        if (card.cardType == Card.CARDTYPE.SPELL)
        {
            CardDeath();
        }

    }

    //--------------------------------------------------- Modify UI ---------------------------------------------------//

    //Updates this Card's UI stat text fields with new/current stats
    private void UpdateCardUI()
    {
        UpdateCardUI(card.currentEnergyCost, card.currentHealth, card.currentAttack);
    }
    private void UpdateCardUI(int manaCost, int health, int damage)
    {
        manaCostText.text = manaCost.ToString();
        if (card.cardType == Card.CARDTYPE.SPELL || card.cardType == Card.CARDTYPE.FIELD || card.cardType == Card.CARDTYPE.ENCHANTMENT)
        {
            healthText.text = "";
            attackText.text = "";
        }
        else
        {
            healthText.text = health.ToString();
            attackText.text = damage.ToString();
        }

        if (HasCondition(ConditionType.DivineShield))
        {
            divineShieldArt.gameObject.SetActive(true);
        }
        else
        {
            divineShieldArt.gameObject.SetActive(false);
        }

        if (HasCondition(ConditionType.Tough) && transform.parent != null && transform.parent.CompareTag("FieldSlot"))
        {
            toughArt.gameObject.SetActive(true);
        }
        else
        {
            toughArt.gameObject.SetActive(false);
        }


        //TODO: Change Colour of circle if current /= base. otherwise set colour to normal
    }
    private void SetCardUI(Sprite borderImage, Sprite characterArtImage, string cardName, string cardDescription, int manaCost, int health, int damage, Card.CARDRARITY cardRarity)
    {
        cardNameText.text = cardName;
        cardDescriptionText.text = cardDescription;
        //SetsImage
        cardBorderImage.overrideSprite = borderImage;
        cardCharacterArtImage.overrideSprite = characterArtImage;


        //Applies stat fields with values
        UpdateCardUI(manaCost, health, damage);

        //Set Game Object's Name
        this.gameObject.name = cardName;

        //Hides Energy if Horde
        if (card.cardAllegiance == Card.ALLEGIANCE.HORDE)
        {
            manaCostText.gameObject.SetActive(false);
        }
        if (card.cardAllegiance == Card.ALLEGIANCE.PLAYER)
        {
            manaCostText.gameObject.SetActive(true);
        }

        // Disable all rarity images first just in case
        rarityB.gameObject.SetActive(false);
        rarityC.gameObject.SetActive(false);
        rarityR.gameObject.SetActive(false);
        rarityE.gameObject.SetActive(false);
        rarityL.gameObject.SetActive(false);

        // Enable the correct rarity image based on the card's rarity
        switch (cardRarity)
        {
            case Card.CARDRARITY.NONE:
                break;
            case Card.CARDRARITY.BASIC:
                rarityB.gameObject.SetActive(true);
                break;
            case Card.CARDRARITY.COMMON:
                rarityC.gameObject.SetActive(true);
                break;
            case Card.CARDRARITY.RARE:
                rarityR.gameObject.SetActive(true);
                break;
            case Card.CARDRARITY.EPIC:
                rarityE.gameObject.SetActive(true);
                break;
            case Card.CARDRARITY.LEGENDARY:
                rarityL.gameObject.SetActive(true);
                break;
            default:
                Debug.LogWarning("Unknown rarity type.");
                break;
        }

    }

}
