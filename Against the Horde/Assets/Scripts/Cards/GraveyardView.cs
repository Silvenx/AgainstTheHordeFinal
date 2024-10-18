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
    public int cardsPerRow = 6;

    void Start()
    {
        //Hide the graveyard at the start if it's not already
        graveyardBacking.SetActive(false);

        //Setting up the Grid Layout Group to arrange the cards
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = cardsPerRow; //Set to variable per row
    }

    public void ToggleGraveyard()
    {
        if (graveyardBacking.activeSelf)
        {
            HideGraveyard();
        }
        else
        {
            ShowGraveyard();
        }
    }


    public void ShowGraveyard()
    //Shows the Graveyard
    {


        //Show the card and set it to the graveyard viewer
        foreach (Transform cardTransform in graveyardObject.transform)
        {
            //Set the card to active and set the parent to the graveyard viewer container
            cardTransform.gameObject.SetActive(true);
            cardTransform.SetParent(cardContainer, false);

            cardTransform.localPosition = Vector3.zero;
            cardTransform.localRotation = Quaternion.identity;
            cardTransform.localScale = Vector3.one;

        }
        //Show the graveyard UI
        graveyardBacking.SetActive(true);
    }

    public void HideGraveyard()
    {
        graveyardBacking.SetActive(false);
        foreach (Transform cardTransform in cardContainer)
        {
            // Set the parent back to graveyardObject
            cardTransform.SetParent(graveyardObject.transform, false);

            // Set the card to inactive
            cardTransform.gameObject.SetActive(false);
        }
    }
}
