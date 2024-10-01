using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Json;

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

    public enum TurnPhase
    {
        //New round begins
        STARTROUND,
        //Automated "start of turn" effects trigger
        STANDBY,
        //Player draws card
        DRAW,
        //NPC draws and plays card. If player can react to spell, allow so
        HORDEDRAWANDPLAY,
        //Player can play cards
        PLAY,
        //Player chooses to end turn
        PLAYEREND,
        //Combat between all monsters ensues & lifepoint damage
        COMBAT,
        //Final turn resolutions
        ROUNDEND
    }

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
    }

    //-----------------------------------------------------------------------------------------------------//

    //----------------------------------------Game Round Management----------------------------------------//

    //Cycles to next turn phase
    public void NextTurnPhase()
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

    //Manually triggers specific turnphase
    public void SetTurnPhase(TurnPhase newTurnPhase)
    {
        currentTurn = newTurnPhase;
        TriggerTurnPhasesMethod(newTurnPhase);
    }
    private void TriggerTurnPhasesMethod(TurnPhase newTurnPhase)
    {
        switch (currentTurn)
        {
            case TurnPhase.STARTROUND:
                EnterNewRound();
                break;

            case TurnPhase.HORDEDRAWANDPLAY:
                HordeDrawAndPlay();
                break;

            case TurnPhase.DRAW:
                PlayerDraw();
                break;

            case TurnPhase.STANDBY:
                StartOfTurnEffectTrigger();
                break;

            case TurnPhase.PLAY:
                PlayerGainControl();
                break;

            case TurnPhase.PLAYEREND:
                EndOfTurnEffectTrigger();
                break;

            case TurnPhase.COMBAT:
                CombatPhase();
                break;

            case TurnPhase.ROUNDEND:
                EndOfRound();
                break;

            default:
                break;
        }
    }

    //--------------------------------------------------- ROUND LOGIC ---------------------------------------------------//


    private void EnterNewRound()
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

        //Next Phase (Automatic)
        NextTurnPhase();
    }
    private void HordeDrawAndPlay()
    {
        //Horde Draw
        hordeManager.HordePlayFromDeck();
        //If card is spell & player has card that can "react", pause turn, trigger PlayerReachToSpell()

        //Next Phase (Automatic)
        NextTurnPhase();
    }
    private void PlayerReactToSpell()
    {
        //Check if horde played spell
        //Check if player has card in hand that has "react" property

    }

    private void PlayerDraw()
    {
        //Draw top card from deck
        playerManager.DrawCardFromTopOfDeck();

        //Next Phase (Automatic)
        NextTurnPhase();
    }

    private void StartOfTurnEffectTrigger()
    {
        //DO STUFF

        //Next Phase (Automatic)
        NextTurnPhase();
    }
    private void PlayerGainControl()
    {
        //DO STUFF
        foreach (GameObject card in playerManager.playerHand)
        {
            card.GetComponent<CardDetails>().canDrag = true;
        }

    }
    private void EndOfTurnEffectTrigger()
    {
        //Remove player's ability to control cards
        //Trigger card effects in play that are "end of turn"

        //Next Phase (Automatic)
        NextTurnPhase();
    }
    private void CombatPhase()
    {
        //Triggers Combat Timers and combat etc
        StartCoroutine(CombatPhaseCoroutine());

        //Next Phase (Automatic)
        //NextTurnPhase(); //JP 28.09.24 - Removed, iIn the Coroutine now
    }

    private IEnumerator CombatPhaseCoroutine()
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
        NextTurnPhase();
    }


    private void EndOfRound()
    {
        //DO STUFF

        //Next Phase (Automatic)
        NextTurnPhase();
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

        //FUTURE: Quick would need to be done here as an if statement
        //Deal damage to eachother at the same time
        playerCard.ModifyCurrentHealth(-hordeCardPower);
        hordeCard.ModifyCurrentHealth(-playerCardPower);

        //on kill trigger probably should go in ModifyHealth
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

    //--------------------------------------------------- MISC ---------------------------------------------------//

}
