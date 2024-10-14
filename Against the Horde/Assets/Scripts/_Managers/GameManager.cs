using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour

{
    public static GameManager Instance { get; private set; } //JP 29.09.24 - added for card db

    [Header("TEMP: TO BE REPLACED AT FULL GAME")]
    public DeckObjects playerStartDeck;
    public DeckObjects hordeStartDeck;

    [Header("Managers")]
    public FieldManager fieldManager;
    public PlayerManager playerManager;
    public HordeManager hordeManager;
    public Canvas myCanvas;
    public GameObject cardPrefab;

    public CardDatabase cardDatabase; //JP 29.09.24 - added for card db 

    //[HideInInspector]
    public GameObject cardObjectPool;


    [Header("Turn Details")]
    public int turnCount = 0;
    public TurnPhase currentTurn = TurnPhase.STARTROUND;
    public int maxEnergy;
    public int currentEnergy = 0;
    public int energyGrowthCap = 10;
    public TextMeshProUGUI currentEnergyText;
    public TextMeshProUGUI maxEnergyText;
    public TextMeshProUGUI turnCountText;

    [Header("UI Objects")]
    public GameObject mulliganArea;

    public enum TurnPhase
    {
        STARTROUND,         //Starts a new round, triggers On turn start card effects
        MULLIGAN,           //On the first turn, the player may discard and redraw cards
        HORDEDRAWANDPLAY,   //Horde draws their cards and plays it, activates on draw effects, reactions would probably go here
        PLAYERDRAW,         //Player draws cards, activates on draw effects
        STANDBY,            //Reserved - not used yet
        PLAYERPLAY,         //Player plays cards at this phase and chooses when to end turn
        COMBAT,             //Combat occurs, left to right at the same time
        PLAYEREND,          //End of turn effects activate
        ROUNDEND            //Goes to next round, reserved for the future
    }

    [Header("CursorSkin")]
    public Texture2D mainCursorTexture;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    [Header("Misc")]
    public GameObject magnifiedCard;
    public float timeToWaitForMagnifiedCard = 1.5f;
    public float highlightCardRaiseDistance = 10f;

    private void Awake()
    {
        //JP 29.09.24 - Added for the Card Database - maybe not worth it
        // Initialize the singleton instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps the GameManager alive across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }

        // Ensure PlayerManager is assigned
        if (playerManager == null)
        {
            playerManager = FindObjectOfType<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogError("PlayerManager not found in the scene.");
            }
        }
        // Ensure CardDatabase is assigned
        if (cardDatabase == null)
        {
            Debug.LogError("CardDatabase is not assigned in GameManager.");
        }

        CreateCardObjectPool(50);

    }
    private void CreateCardObjectPool(int amountToCreate)
    {
        for (int i = 0; i < amountToCreate; i++)
        {
            GameObject o = Instantiate(cardPrefab, cardObjectPool.transform);
            o.SetActive(false);
        }
    }
    public GameObject CreateCardObject(Card card)
    {
        //Grabs card from Object Pool
        GameObject cardObject = cardObjectPool.transform.GetChild(0).gameObject;
        //Populate Card's details on Game Object
        cardObject.GetComponent<CardDetails>().SetCardDetails(card);
        //Change's Game Object's name
        cardObject.name = "Card_" + card.cardName;
        //Applies card to UI

        //Enables the Object
        cardObject.SetActive(true);

        return cardObject;
    }

    void Start()
    {
        //Sets up player's deck, draws hand
        playerManager.GameSetup(playerStartDeck);

        //Set up player Lifeforce
        int testvalue = 30; //fix this later just testing
        playerManager.SetPlayerLifeForce(testvalue);
        hordeManager.SetHordeLifeForce(testvalue);
        //Set hordes up too

        //Sets up horde's deck
        hordeManager.GameSetup(hordeStartDeck);

        //Enters the first round of the game
        SetTurnPhase(TurnPhase.STARTROUND);

        //Set the default cursor
        SetMainCursorSkin();
    }

    //-----------------------------------------------------------------------------------------------------//

    //----------------------------------------Game Round Management----------------------------------------//

    public void NextPhase()
    //Cycles to next turn phase
    {
        //If at final turn phase
        if (currentTurn == TurnPhase.ROUNDEND)
        {
            //Cycle to first turn phase
            currentTurn = 0;
        }
        else
        {
            //Cycle to next turn phase
            currentTurn += 1;
        }

        //Trigger Methods
        TriggerTurnPhasesMethod(currentTurn);
    }

    public void SetTurnPhase(TurnPhase newTurnPhase)
    //Manually triggers specific turnphase
    {
        currentTurn = newTurnPhase;
        TriggerTurnPhasesMethod(newTurnPhase);
    }

    private void TriggerTurnPhasesMethod(TurnPhase newTurnPhase)
    {
        Debug.Log($"Turn {turnCount}, Phase: {currentTurn}"); // Log current turn phase
        switch (currentTurn)
        {
            case TurnPhase.STARTROUND:
                PhaseStartRound();
                break;

            case TurnPhase.MULLIGAN:
                PhaseMulligan();
                break;

            case TurnPhase.HORDEDRAWANDPLAY:
                PhaseHordeDrawAndPlay();
                break;

            case TurnPhase.PLAYERDRAW:
                PhasePlayerDraw();
                break;

            case TurnPhase.STANDBY:
                PhaseStandby();
                break;

            case TurnPhase.PLAYERPLAY:
                PhasePlayerPlay();
                break;

            case TurnPhase.COMBAT:
                PhaseCombat();
                break;

            case TurnPhase.PLAYEREND:
                PhasePlayerEnd();
                break;

            case TurnPhase.ROUNDEND:
                PhaseRoundEnd();
                break;

            default:
                break;
        }
    }

    //--------------------------------------------------- ROUND LOGIC ---------------------------------------------------//

    private void PhaseStartRound()
    {
        //Increase Round Counter
        turnCount += 1;
        //Increase Player Mana Cap if it's under the growth cap
        if (maxEnergy < energyGrowthCap)
        {
            maxEnergy += 1;
        }
        //Sets refreshes current mana value and updates UI
        SetCurrentEnergy(maxEnergy);
        maxEnergyText.text = maxEnergy.ToString();

        //FUTURE: Add start of turn effects

        //Next Phase (Automatic)
        NextPhase();
    }

    private void PhaseMulligan()
    //Handles the Mulligan of the player's hand on turn one
    {
        if (turnCount == 1)
        {
            StartCoroutine(PhaseMulliganCoroutine());
        }
        else if (turnCount > 1)
        {
            NextPhase();
        }
    }

    private IEnumerator PhaseMulliganCoroutine()
    //Handles the mulligan process
    {

        int finalStartingHandSize = playerManager.startingHandSize - 1;

        //Starting hand size is how many cards are there prior to the mull
        //Draw the first set of cards without the pity 1 drop
        playerManager.DrawCardFromTopOfDeck(finalStartingHandSize);
        //Draws the pity one drop or a regular card
        playerManager.DrawCardEnsuringOneCostCard();

        yield return new WaitForSeconds(1.5f);

        NextPhase();

    }


    private void PhaseHordeDrawAndPlay()
    {
        //Horde Draw
        hordeManager.HordePlayFromDeck();
        //If card is spell & player has card that can "react", pause turn, trigger PlayerReachToSpell()

        //Next Phase (Automatic)
        NextPhase();
    }
    private void PlayerReactToSpell()
    {
        //Check if horde played spell
        //Check if player has card in hand that has "react" property

    }

    private void PhasePlayerDraw()
    {
        //Draw top card from deck
        playerManager.DrawCardFromTopOfDeck();

        //Next Phase (Automatic)
        NextPhase();
    }

    private void PhaseStandby()
    {
        //Turn start won't be here
        //Reserved for maybe later use

        //Next Phase (Automatic)
        NextPhase();
    }
    private void PhasePlayerPlay()
    {
        //DO STUFF
        foreach (GameObject card in playerManager.playerHand)
        {
            card.GetComponent<CardDetails>().canDrag = true;
        }

    }
    private void PhasePlayerEnd()
    {
        //FUTURE: Remove player's ability to control cards

        //Go through all player monsters left to right and apply end of round effects
        CardDetails cardDetails = GameObject.FindObjectOfType<CardDetails>();

        foreach (GameObject playerSlot in fieldManager.playerMonsterSlots)
        {
            if (playerSlot.transform.childCount > 0)
            {
                CardDetails playerCard = playerSlot.transform.GetChild(0).GetComponent<CardDetails>();

                //Should do bleeding, then burning, then regenerate and combine together then change health so heal cancels out burn etc
                int regenValue = 0;
                int regenHealing = 0;

                int burnValue = 0;
                int burnDamage = 0;

                int bleedValue = 0;
                int bleedDamage = 0;

                if (playerCard.HasCondition(ConditionType.Burn))
                {
                    //FUTURE: Burn hitsplat value, but want the actual number to change at the same time
                    //Get the burn value then double it for the effect
                    burnValue = playerCard.GetConditionValue(ConditionType.Burn);
                    burnDamage = burnValue * 2;

                    //Reduce the burn stack on the card by 2
                    burnValue -= 2;
                    playerCard.ModifyConditionValue(ConditionType.Burn, burnValue);
                }

                if (playerCard.HasCondition(ConditionType.Bleed))
                {
                    //FUTURE: Bleed hitsplat value, but want the actual number to change at the same time
                    //Get the bleed value
                    bleedValue = playerCard.GetConditionValue(ConditionType.Bleed);
                    bleedDamage = bleedValue;

                    //Reduce the bleed stack on the card by 1
                    bleedValue -= 1;
                    playerCard.ModifyConditionValue(ConditionType.Bleed, bleedValue);
                }

                //Check if it has regeneration, regen if it does, then reduce it by 1
                if (playerCard.HasCondition(ConditionType.Regenerate))
                {
                    //FUTURE: Regen healing value, but want the actual number to change at the same time
                    //Get the regen value
                    regenValue = playerCard.GetConditionValue(ConditionType.Regenerate);
                    regenHealing = regenValue;

                    //reduce the regen stack on the card by 1
                    int finalRegenValue = regenValue - 1;
                    playerCard.ModifyConditionValue(ConditionType.Regenerate, finalRegenValue);
                }

                //Final condition calculations
                int lifeAdjustment = regenHealing - burnDamage - bleedDamage;

                //If life adjustment is healing
                if (lifeAdjustment > 0)
                {
                    playerCard.HealLife(lifeAdjustment);
                }

                //If life adjustment is damage
                if (lifeAdjustment < 0)
                {
                    playerCard.TakeLifeDamage(lifeAdjustment);
                }

                //Activate end turn effects
                playerCard.ActivateCardEffect(TriggerType.ENDTURN);
            }
        }

        foreach (GameObject hordeSlot in fieldManager.hordeMonsterSlots)
        {
            if (hordeSlot.transform.childCount > 0)
            {
                CardDetails hordeCard = hordeSlot.transform.GetChild(0).GetComponent<CardDetails>();

                //Check if it has regeneration, regen if it does, then reduce it by 1
                if (hordeCard.HasCondition(ConditionType.Regenerate))
                {
                    int regenValue = hordeCard.GetConditionValue(ConditionType.Regenerate);
                    hordeCard.HealLife(regenValue);
                    int newRegenValue = regenValue - 1;
                    hordeCard.ModifyConditionValue(ConditionType.Regenerate, newRegenValue);
                }

                hordeCard.ActivateCardEffect(TriggerType.ENDTURN);
            }
        }

        if (fieldManager.fieldCardSlot.transform.childCount > 0)
        {
            CardDetails fieldCard = fieldManager.fieldCardSlot.transform.GetChild(0).GetComponent<CardDetails>();
            fieldCard.ActivateCardEffect(TriggerType.ENDTURN);
        }

        //Next Phase (Automatic)
        NextPhase();
    }
    private void PhaseCombat()
    {
        //Triggers Combat Timers and combat etc
        StartCoroutine(PhaseCombatCoroutine());

        //NextTurnPhase(); //JP 28.09.24 - Removed, in the CombatPhaseCoroutine Coroutine now
    }

    private IEnumerator PhaseCombatCoroutine()
    {
        //Loop through each player and horde monster slot
        for (int i = 0; i < fieldManager.playerMonsterSlots.Count; i++)
        {
            // Get the player and horde slots at the current index (from 1 to 5, as you have 5 slots)
            GameObject playerSlot = fieldManager.playerMonsterSlots[i];
            GameObject hordeSlot = fieldManager.hordeMonsterSlots[i];

            // Check if there's a monster in the player's slot, then check horde
            GameObject playerCardObject = playerSlot.transform.childCount > 0 ? playerSlot.transform.GetChild(0).gameObject : null;
            GameObject hordeCardObject = hordeSlot.transform.childCount > 0 ? hordeSlot.transform.GetChild(0).gameObject : null;

            //FUTURE: On attack trigger needs to go here somewhere

            //If both players have monsters in the same slot
            if (playerCardObject != null && hordeCardObject != null)
            {
                CardDetails playerCardDetails = playerCardObject.GetComponent<CardDetails>();
                CardDetails hordeCardDetails = hordeCardObject.GetComponent<CardDetails>();

                Debug.Log($"Slot {i + 1} - Player's {playerCardDetails.card.cardName} fights Horde's {hordeCardDetails.card.cardName}");

                MonsterCombat(playerCardDetails, hordeCardDetails);
            }

            //If player has one but horde doesn't
            else if (playerCardObject != null && hordeCardObject == null)
            {
                CardDetails playerCardDetails = playerCardObject.GetComponent<CardDetails>();

                Debug.Log($"Slot {i + 1} - Player's {playerCardDetails.card.cardName} hits face");

                PlayerMonsterDealsLifeForceDamage(playerCardDetails);

            }
            //If horde has one but player doesn't
            else if (playerCardObject == null && hordeCardObject != null)
            {
                CardDetails hordeCardDetails = hordeCardObject.GetComponent<CardDetails>();

                Debug.Log($"Slot {i + 1} - Horde's {hordeCardDetails.card.cardName} hits face");

                HordeMonsterDealsLifeForceDamage(hordeCardDetails);
            }
            else
            {
                Debug.Log($"Slot {i + 1} - No monsters present on either side.");
            }
            yield return new WaitForSeconds(0.5f);
        }
        NextPhase();
    }

    private void PhaseRoundEnd()
    {
        //Next Phase (Automatic)
        NextPhase();
    }

    //--------------------------------------------------- ENERGY CHECKS ---------------------------------------------------//

    //Check if have enough mana to play card
    public bool haveEnoughManaToPlayCard(Card card)
    {
        if (card.currentEnergyCost <= currentEnergy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //Changes current energy by X amount
    public void ModifyCurrentEnergy(int amountToIncrease)
    {
        //increases current mana
        int newCurrentEnergy = currentEnergy + amountToIncrease;
        //Updates var and text UI
        SetCurrentEnergy(newCurrentEnergy);
    }
    //Updates current energy variable and UI
    public void SetCurrentEnergy(int newCurrentEnergyValue)
    {
        currentEnergy = newCurrentEnergyValue;
        //Updates text UI element
        currentEnergyText.text = currentEnergy.ToString();
    }

    //Updates current energy variable and UI
    public void SetMaxEnergy(int newMaxEnergyValue, bool isDebug)
    {
        if (!isDebug)
        {
            if (newMaxEnergyValue > 10)
            {
                newMaxEnergyValue = 10;
            }
            maxEnergy = newMaxEnergyValue;
            maxEnergyText.text = maxEnergy.ToString();
        }
        else if (isDebug)
        {
            maxEnergy = newMaxEnergyValue;
            maxEnergyText.text = maxEnergy.ToString();
        }
        Debug.Log("Max Energy set to: " + maxEnergy);
    }

    //--------------------------------------------------- COMBAT PROCESSING ---------------------------------------------------//

    //Processes the combat between monsters
    public void MonsterCombat(CardDetails playerCard, CardDetails hordeCard)
    {
        //FUTURE: probably need to add in a check before the damage dealt because onattack may kill a monster already, ie. Drain

        // Trigger OnAttack event for player card
        playerCard.ActivateCardEffect(TriggerType.ATTACK);

        // Trigger OnAttack event for horde card
        hordeCard.ActivateCardEffect(TriggerType.ATTACK);

        int playerCardPower = playerCard.card.currentAttack;
        int hordeCardPower = hordeCard.card.currentAttack;
        int hordeRetaliateStacks = hordeCard.GetConditionValue(ConditionType.Retaliate);
        int playerRetaliateStacks = playerCard.GetConditionValue(ConditionType.Retaliate);


        //If both player and horde have quick or niether do
        if (playerCard.HasCondition(ConditionType.Quick) && hordeCard.HasCondition(ConditionType.Quick) ||
        playerCard.HasCondition(ConditionType.Quick) == false && hordeCard.HasCondition(ConditionType.Quick) == false)
        {
            //Deal damage to both at the same time
            playerCard.TakeLifeDamage(hordeCardPower);
            hordeCard.TakeLifeDamage(playerCardPower);
            //Retaliate happens immediately after and is separate
            playerCard.TakeLifeDamage(hordeRetaliateStacks);
            hordeCard.TakeLifeDamage(playerRetaliateStacks);

        }
        //If Player has quick and Horde Doesn't
        else if (playerCard.HasCondition(ConditionType.Quick) && hordeCard.HasCondition(ConditionType.Quick) == false)
        {
            //Deal damage to the horde card
            hordeCard.TakeLifeDamage(playerCardPower);
            //Player still takes retaliate damage, not avoided by quick
            playerCard.TakeLifeDamage(hordeRetaliateStacks);

        }
        //If horde has quick and player doesn't
        else if (hordeCard.HasCondition(ConditionType.Quick) && playerCard.HasCondition(ConditionType.Quick) == false)
        {
            //Deal damage to the horde card
            playerCard.TakeLifeDamage(hordeCardPower);
            //Horde still takes retaliate damage, not avoided by quick
            hordeCard.TakeLifeDamage(playerRetaliateStacks);
        }
        //FUTURE: Add in on kill here i think
    }

    //Process horde dealing face damage with a monster
    public void PlayerMonsterDealsLifeForceDamage(CardDetails playerCard)
    {
        // Trigger OnAttack event for player card
        playerCard.ActivateCardEffect(TriggerType.ATTACK);

        //Get the player card current attack
        playerCard.GetComponent<CardDetails>();
        int damage = playerCard.card.currentAttack;

        //Apply it inversely to the horde lifeforce
        hordeManager.ModifyCharacterLifeForce(-damage);
    }

    //Process horde dealing face damage with a monster
    public void HordeMonsterDealsLifeForceDamage(CardDetails hordeCard)
    {
        // Trigger OnAttack event for horde card
        hordeCard.ActivateCardEffect(TriggerType.ATTACK);

        //Get the horde card current attack
        hordeCard.GetComponent<CardDetails>();
        int damage = hordeCard.card.currentAttack;

        //Apply it inversely to the player lifeforce
        playerManager.ModifyCharacterLifeForce(-damage);
    }
    //---------------------------------------------- MOUSE POINTER -----------------------------------------------//

    void SetMainCursorSkin()
    {
        Cursor.SetCursor(mainCursorTexture, hotspot, cursorMode);
    }

    void OnDisable()
    {
        // Reset the cursor to the default if GameManager is disabled or exits
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    //-------------------------------------------------- BUTTONS --------------------------------------------------//

    public void EndTurnButtonPress()
    {
        if (currentTurn == TurnPhase.PLAYERPLAY)
        {
            NextPhase();
        }
        else
        {
            Debug.Log("Can't End Turn, it's not Play Phase");
        }
    }



    //--------------------------------------------------- MISC ----------------------------------------------------//

}
