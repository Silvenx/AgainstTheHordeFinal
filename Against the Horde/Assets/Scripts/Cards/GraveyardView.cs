using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraveyardView : MonoBehaviour
{
    public GameObject graveyardBacking; //The backing of the graveyard
    public GameObject cardPrefab;
    public Transform cardContainer; //
    public GridLayoutGroup gridLayoutGroup; //arranges the cards
    public GameObject graveyardObject; //This object has the cards in the graveyard from the combat etc scripts
    public CardDetails cardDetails;

    void Start()
    {
        //Hide the graveyard at the start if it's not already
        graveyardBacking.SetActive(false);

        //Setting up the Grid Layout Group to arrange the cards
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 6; //Set to 6 cards per row
    }

    public void ShowGraveyard()
    //Shows the Graveyard
    {

        //Clear the existing children in the container first before showing the new ones
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        //Create a new card for each one
        foreach (Transform cardTransform in graveyardObject.transform)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null)
            {
                GameObject newCard = Instantiate(cardPrefab, cardContainer);
                cardDetails = newCard.GetComponent<CardDetails>();
                cardDetails.SetCardDetails(card);
            }

        }
        //Show the graveyard
        graveyardBacking.SetActive(true);
    }

    public void HideGraveyard()
    {
        graveyardBacking.SetActive(false);
    }
}
