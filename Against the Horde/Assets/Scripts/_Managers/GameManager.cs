using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("TEMP: TO BE REPLACED AT FULL GAME")]
    public DeckObjects playerStartDeck;
    public DeckObjects hordeStartDeck;

    [Header("Managers")]
    public FieldManager fieldManager;
    public PlayerManager playerManager;
    public HordeManager hordeManager;
    public Canvas myCanvas;
    public GameObject cardPrefab;
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
    public GameObject RetrieveCardObject(Card cardDetails)
    {
        //Grabs card from Object Pool
        GameObject card = cardObjectPool.transform.GetChild(0).gameObject;
        //Change's Game Object's name
        card.name = "Card_" + cardDetails.cardName;
        //Applies card to UI

        //Enables the Object
        card.SetActive(true);

        return card;
    }

    void Start()
    {
        //Sets up player's deck, draws hand
        playerManager.PlayerGameSetup(playerStartDeck);

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

    //-----------------------------------Round Logic-----------------------------------//


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
        //Trigger fighting between monsters

        //Wait timer until combat finishes

        //Next Phase (Automatic)
        NextTurnPhase();
    }
    private void EndOfRound()
    {
        //DO STUFF

        //Next Phase (Automatic)
        NextTurnPhase();
    }

    //-----------------------------------------------------------------------------------------------------//

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
}
