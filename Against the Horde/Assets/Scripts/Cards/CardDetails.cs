using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDetails : MonoBehaviour
{

    [Header("Card Values")]
    public Card card;

    [Header("UI Fields")]
    public Image cardBorderImage;
    public Image cardCharacterArtImage;
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

        this.card.borderArt = cardDetails.borderArt;
        this.card.characterArt = cardDetails.characterArt;

        this.card.cardAllegiance = cardDetails.cardAllegiance;

        //Applies new card details to UI of this card gameobject
        SetCardUI(card.borderArt, card.characterArt, card.cardName, card.cardDescription, card.baseManaCost, card.baseHealth, card.baseAttack);
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
        return new Card(card.cardName, card.cardType, card.cardDescription, card.baseManaCost, card.baseHealth, card.baseAttack);
    }

    //---------------------Card Functions---------------------

    //Play Card on Field Slot
    public void PlayThisCardOnFieldSlot(GameObject fieldSlot)
    {
        RectTransform rect = GetComponent<RectTransform>();

        //Set parent to be the Field Slot
        transform.SetParent(fieldSlot.GetComponent<RectTransform>());

        //Set position to be same as field slot it is placed under
        rect.anchoredPosition = Vector2.zero;
        //currentCoroutine = CharacterManager.LerpObjectMovement(this.gameObject, rect.anchoredPosition, Vector2.zero, 2f, 1.5f);
        //StartCoroutine(currentCoroutine);
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
        FieldManager.SendCardObjectToGraveyard(this.gameObject, isCardAllegiancePlayer);
    }


    ///----------------------------------------------------------------------------------------------------//
    //----------------------------------------------Modify UI----------------------------------------------//

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

        //TODO: Change Colour of text if current /= base. otherwise set colour to normal
    }
    private void SetCardUI(Sprite borderImage, Sprite characterArtImage, string cardName, string cardDescription, int manaCost, int health, int damage)
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

    }

}
