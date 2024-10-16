using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardEventTrigger : EventTrigger
{
    private GameManager gameManager;
    private PlayerManager playerManager;
    private FieldManager fieldManager;
    private Canvas myCanvas;
    private Card thisCard;
    private bool pointerOverCard = false; ////Branch1.0-Added variable
    //private bool pointOverSpellPlayArea = false;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerManager = FindAnyObjectByType<PlayerManager>();
        fieldManager = FindAnyObjectByType<FieldManager>();
        myCanvas = gameManager.myCanvas;
        thisCard = GetComponent<CardDetails>().card;
        timeToWaitForMagnifiedCard = gameManager.timeToWaitForMagnifiedCard;
    }

    //-------------------------------------------------------------------------//
    //--------------------------------On Drags---------------------------------//

    //Mouse is holding click and starts to move while maintaining the mouse down
    //Called once
    public override void OnBeginDrag(PointerEventData data)
    {
        //Ends Lerp Coroutine if card is current under one
        playerManager.StopLerpCoroutineOnCard(this.gameObject);
        //Hides card if player tries to start dragging it
        HideMagnifiedCard();

        //Ends Drag if card is not interactable
        if (GetComponent<CardDetails>().canDrag == false || gameManager.currentTurn != GameManager.TurnPhase.PLAYERPLAY) { data.pointerDrag = null; return; }

        //Marks card as currently being moved
        playerManager.cardBeingMoved = this.gameObject;

        //SHOW VFX OF POSSIBLE POSITIONS TO PLAY
        fieldManager.HighlightApplicableFieldSlots(true, thisCard);

        //Turn off pokeup animation
        GetComponent<Animator>().SetBool("PokeUp", false);
    }

    //Called on mouseup after OnDrag() ends
    public override void OnEndDrag(PointerEventData data)
    {
        PlayDraggedCard();
    }

    //Called every frame during a drag
    public override void OnDrag(PointerEventData data)
    {
        ObjectFollowMousePointer();
    }

    //----------------------On Drag Methods----------------------//

    private void PlayDraggedCard()
    {
        //Checks if player has enough mana to play this card
        if (gameManager.haveEnoughManaToPlayCard(thisCard))
        {

            if (thisCard.cardType == Card.CARDTYPE.MONSTER)
            {
                //Checks if slot is a viable slot to place card on.
                //Placing it down if so, or returning it to hand if not.
                CheckAndPlaceCardOnSlot();
            }

            if (thisCard.cardType == Card.CARDTYPE.SPELL || thisCard.cardType == Card.CARDTYPE.FIELD || thisCard.cardType == Card.CARDTYPE.ENCHANTMENT)
            {
                CheckAndPlaceSpellSlot();
            }
        }
        else
        {
            //Place card back into hand
            playerManager.OrganiseHand(playerManager.cardMoveSpeed * 2f);
        }

        //Removes Drag animation
        GetComponent<Animator>().SetBool("PokeUp", false);

        //DISABLE Field Highlight VFX
        fieldManager.DisableHighlightsForAllFieldSlots();
    }

    private void CheckAndPlaceCardOnSlot()
    {
        //CHECK IF CARD IS PLACED IN APPLICABLE AREA
        if (fieldManager.canPlayCardOnThisFieldSlot(thisCard, selectedFieldSlot))
        {
            ///Position Card in Field
            //Remove from hand
            playerManager.RemoveCardFromHand(this.gameObject);
            CardDetails cDetails = GetComponent<CardDetails>();
            //Plays card under Field Manager Slot
            cDetails.PlayThisCardOnFieldSlot(selectedFieldSlot);


            ///Spend Mana & disable ability to drag this card
            //Spends mana equal to card's current mana cost
            gameManager.ModifyCurrentEnergy(-thisCard.currentEnergyCost);
            //Make it so player can't drag card anymore
            cDetails.canDrag = false;


            ///Trigger Card's OnPlay() Effects
            //TODO
        }
        //Card could not be activated
        else
        {
            //Reset cards to position
            playerManager.OrganiseHand(playerManager.cardMoveSpeed * 2f);
        }
    }

    private void CheckAndPlaceSpellSlot()
    {
        //Grab the Spell play area
        GameObject spellPlayArea = GameObject.Find("Field_SpellPlayArea");
        GameObject spellHoldArea = GameObject.Find("Field_SpellHold");
        CardDetails cDetails = GetComponent<CardDetails>();

        //Get mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //2D raycast
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);
        Debug.Log("Number of objects hit: " + hits.Length);

        bool spellPlaced = false;

        //I think check mana and spell type here? or nah

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject == spellPlayArea)
            {
                Debug.Log("Spell found the play area");

                //remove from hand
                playerManager.RemoveCardFromHand(this.gameObject);

                //Need to check here what type of spell
                bool requiresManualTargeting = false;
                foreach (var ability in thisCard.abilities)
                {
                    if (ability.target is Tg_ManualSelect)
                    {
                        requiresManualTargeting = true;
                        break;
                    }
                }

                if (requiresManualTargeting)
                {
                    //Move card to spell hold area
                    transform.SetParent(spellHoldArea.transform, false);
                    transform.localPosition = Vector3.zero;
                    cDetails.PlaySpellCard();
                    //cDetails.CardDeath(); - moved to playspell
                }
                // Disable dragging
                cDetails.canDrag = false;

                // Spend Mana
                gameManager.ModifyCurrentEnergy(-thisCard.currentEnergyCost);

                spellPlaced = true;
                break;
            }
        }
        if (!spellPlaced)
        {
            Debug.Log("spell failed to hit the area");
            playerManager.OrganiseHand(playerManager.cardMoveSpeed * 2f);
        }
    }

    private void ObjectFollowMousePointer()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);
    }

    //----------------Card Enter Field Slot Events----------------//

    private GameObject selectedFieldSlot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FieldSlot"))
        {
            selectedFieldSlot = collision.gameObject;
        }

        /*JP 06.10.24 - Using Raycasting instead, so removed this logic
        // Check if we entered the spell play area
        if (collision.gameObject.CompareTag("SpellPlayArea"))
        {
            pointOverSpellPlayArea = true;
        }*/
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if we exited a field slot
        if (collision.gameObject.CompareTag("FieldSlot"))
        {
            if (selectedFieldSlot == collision.gameObject)
            {
                selectedFieldSlot = null;
            }
        }
        // Check if we exited the spell play area
        /*if (collision.gameObject.CompareTag("SpellPlayArea"))
        {
            pointOverSpellPlayArea = false;
        }*/
    }

    //--------------------------------On Clicks--------------------------------//

    private float timeToWaitForMagnifiedCard;
    private Coroutine magnifiedCoroutine = null;
    //Mouse enters card's raycast area
    public override void OnPointerEnter(PointerEventData data)
    {
        //If card exists in hand && is play phase
        if (playerManager.playerHand.Contains(this.gameObject) && gameManager.currentTurn == GameManager.TurnPhase.PLAYERPLAY)
        {
            //Lerp card up slightly if card is in hand
            GetComponent<Animator>().SetBool("PokeUp", true);
        }

        pointerOverCard = true; ////Branch1.0-Hover true

        //Mousing over card will display a magnified version after X sec
        //MouseOverCardShowsMagnifiedVersion(this.gameObject); //Branch1.0-MovetoRightClick
    }


    //Mouse exits card's raycast area
    public override void OnPointerExit(PointerEventData data)
    {
        //Cancels timer for magnified card display
        if (magnifiedCoroutine != null) { StopCoroutine(magnifiedCoroutine); }

        //Hides magnified card display
        //HideMagnifiedCard(); //Branch1.0-MovetoRightClick

        //Move cards back into position
        GetComponent<Animator>().SetBool("PokeUp", false);

        pointerOverCard = false; ////Branch1.0-Hover false
    }

    //Branch1.0-RightClick to magnify
    private void Update()
    {
        GameObject magCard = gameManager.magnifiedCard;

        if (pointerOverCard && Input.GetMouseButtonDown(1)) //hover + right click
        {
            if (!magCard.activeSelf)
            {
                DisplayMagnifiedCard();
            }
            else
            {
                HideMagnifiedCard();
            }
        }

    }
    //----------------Pointer Enter/Exit Methods----------------//

    private void MouseOverCardShowsMagnifiedVersion(GameObject cardObject)
    {
        //Start timer
        magnifiedCoroutine = StartCoroutine(CountdownUntilMagnifiedCardDisplayed(timeToWaitForMagnifiedCard));
    }

    IEnumerator CountdownUntilMagnifiedCardDisplayed(float timeToWait)
    {
        float counter = timeToWait;
        while (counter > 0f)
        {
            counter -= Time.deltaTime;
            yield return null;
        }

        //Displays Magnified Card
        DisplayMagnifiedCard();
    }

    private void DisplayMagnifiedCard()
    {
        GameObject magCard = gameManager.magnifiedCard;

        //Update Magnified Card with card details
        magCard.GetComponent<CardDetails>().SetCardDetails(thisCard);

        //Enables game object
        magCard.SetActive(true);
    }

    private void HideMagnifiedCard()
    {
        GameObject magCard = gameManager.magnifiedCard;

        //Disables game object
        magCard.SetActive(false);
    }




    //----------------------------------------------------------//


    //Use does a mouse up & down within 0.X sec (any mouse input)
    //Occurs alongside up and down events
    public override void OnPointerClick(PointerEventData data)
    {
        //Debug.Log("OnPointerClick called.");
        playerManager.objectsUnderMouseOnClickCardObject = data.hovered;
        //Debug.Log("Last Press Name = " + data.pointerClick.name);
    }


    //Mouse clicks down (any mouse input) while in card's raycast area
    public override void OnPointerDown(PointerEventData data)
    {
        //Debug.Log("OnPointerDown called.");
    }

    //Mouse clicks up (any mouse input) while in card's raycast area
    public override void OnPointerUp(PointerEventData data)
    {
        //Debug.Log("OnPointerUp called.");
    }

    //-------------------------------------------------------------------------//

    //public override void OnCancel(BaseEventData data)
    //{
    //    Debug.Log("OnCancel called.");
    //}

    //public override void OnDeselect(BaseEventData data)
    //{
    //    Debug.Log("OnDeselect called.");
    //}

    //public override void OnDrop(PointerEventData data)
    //{
    //    Debug.Log("OnDrop called.");
    //}

    //public override void OnInitializePotentialDrag(PointerEventData data)
    //{
    //    Debug.Log("OnInitializePotentialDrag called.");
    //}

    //public override void OnMove(AxisEventData data)
    //{
    //    Debug.Log("OnMove called.");
    //}

    //public override void OnScroll(PointerEventData data)
    //{
    //    Debug.Log("OnScroll called.");
    //}

    //public override void OnSelect(BaseEventData data)
    //{
    //    Debug.Log("OnSelect called.");
    //}

    //public override void OnSubmit(BaseEventData data)
    //{
    //    Debug.Log("OnSubmit called.");
    //}

    //public override void OnUpdateSelected(BaseEventData data)
    //{
    //    Debug.Log("OnUpdateSelected called.");
    //}
}
