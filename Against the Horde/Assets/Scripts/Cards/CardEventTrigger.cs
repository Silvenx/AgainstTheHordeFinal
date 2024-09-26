using System.Collections;
using System.Collections.Generic;
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
        if (GetComponent<CardDetails>().canDrag == false || gameManager.currentTurn != GameManager.TurnPhase.PLAY) { data.pointerDrag = null; return; }

        //Marks card as currently being moved
        playerManager.cardBeingMoved = this.gameObject;

        //SHOW VFX OF POSSIBLE POSITIONS TO PLAY
        fieldManager.HighlightApplicableFieldSlots(true, thisCard);
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
            ///Checks if slot is a viable slot to place card on.
            //Placing it down if so, or returning it to hand if not.
            CheckAndPlaceCardOnSlot();
        }
        else
        {
            //Place card back into hand
            playerManager.OrganiseHand(playerManager.cardMoveSpeed * 2f);
        }

        //DISABLE Field Highlight VFX
        fieldManager.DisableHighlightsForAllFieldSlots();
    }

    private void CheckAndPlaceCardOnSlot()
    {
        //Debug.Log("OnEndDrag called.");
        RectTransform rect = GetComponent<RectTransform>();


        //CHECK IF CARD IS PLACED IN APPLICABLE AREA
        if (fieldManager.canPlayCardOnThisFieldSlot(thisCard, selectedFieldSlot))
        {
            ///Position Card in Field
            //Remove from hand
            playerManager.RemoveCardFromHand(this.gameObject);
            //Set parent to be the Field Slot
            transform.SetParent(selectedFieldSlot.GetComponent<RectTransform>());
            //Set position to be same as field parent
            rect.anchoredPosition = Vector2.zero;

            ///Spend Mana & disable ability to drag this card
            //Spends mana equal to card's current mana cost
            gameManager.ModifyCurrentMana(-thisCard.currentManaCost);
            //Make it so player can't drag card anymore
            GetComponent<CardDetails>().canDrag = false;

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


    private void ObjectFollowMousePointer()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);
    }

    //----------------------------------------------------------//

    private GameObject selectedFieldSlot;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        selectedFieldSlot = collision.gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        selectedFieldSlot = null;
    }

    //--------------------------------On Clicks--------------------------------//

    private float timeToWaitForMagnifiedCard;
    private Coroutine magnifiedCoroutine = null;
    //Mouse enters card's raycast area
    public override void OnPointerEnter(PointerEventData data)
    {
        //If card exists in hand && is play phase
        if (playerManager.playerHand.Contains(this.gameObject) && gameManager.currentTurn == GameManager.TurnPhase.PLAY)
        {
            GetComponent<Animator>().SetBool("PokeUp", true);
        }

        pointerOverCard = true; ////Branch1.0-Hover true

        //Lerp card up slightly if card is in hand
        //playerManager.LerpCardUpwardsSlightly(this.gameObject);

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
