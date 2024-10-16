using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Effect_Choose")]
public class Ef_Choose : Effect
{
    // List of effects to do
    public List<ChooseEffects> abilities;

    [System.Serializable]
    public class ChooseEffects
    {
        public Effect effect;
        public Target target;
    }

    //------------------------//

    public override IEnumerator ActivateEffect(Target target, GameObject thisCard)
    {
        //// Start the target selection coroutine and wait for it to complete
        //yield return GameManager.Instance.StartCoroutine(target.TargetAquisition(thisCard));

        // Get the selected targets after selection is complete
        GameObject[] targets = null;

        ThisEffect(targets, thisCard);

        yield return null;
    }

    public override void ThisEffect(GameObject[] targets, GameObject thisCard)
    {
        PlayerManager playerManager = GameManager.Instance.playerManager;
        //Gets Objects
        GameObject blackScreen = playerManager.darkenScreen;
        CardDetails cardDisplay = playerManager.choiceButtonCardDisplay.GetComponent<CardDetails>();
        RectTransform buttonPos = playerManager.choiceButtonsPos;
        GameObject choiceButtonsParent = playerManager.choiceButtonsParent;
        List<GameObject> choiceButtons = playerManager.choiceButtons;


        //Move buttonPos to match card's position on field
        buttonPos.anchoredPosition = thisCard.GetComponent<RectTransform>().anchoredPosition;
        //have dummy card's details update
        cardDisplay.SetCardDetails(thisCard.GetComponent<CardDetails>().card);

        //Turn All Choice Buttons Off
        foreach (GameObject o in choiceButtons)
        {
            o.SetActive(false);
        }
        //Turn on Black Screen
        blackScreen.SetActive(true);

        //List<Button> buttonElements = new List<Button>();
        //Turn on choice buttons (method reside in player manager)
        Debug.Log("Total ability count: " + abilities.Count);
        for (int i = 0; i < abilities.Count; i++)
        {
            Debug.Log("current ability: " + abilities[i].effect.effDesc);
            Debug.Log("i = " + i);

            //Remove past listeners
            choiceButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();

            //Turn on button
            choiceButtons[i].SetActive(true);
            //Set text desc on button
            choiceButtons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = abilities[i].effect.effDesc;
            //Apply effect listener to the button
            int currenti = i;
            choiceButtons[i].GetComponent<Button>().onClick.AddListener(() =>
            { CallEffect(currenti, thisCard); });
            //Add method to hide choice menu
            choiceButtons[i].GetComponent<Button>().onClick.AddListener(() =>
            { HideChoiceMenu(); });
        }
    }

    private void CallEffect(int i, GameObject thisCard)
    {
        Debug.Log("Calling Effect: " + abilities[i].effect);

        GameManager.Instance.StartCoroutine(abilities[i].effect.ActivateEffect(abilities[i].target, thisCard));
    }

    public void HideChoiceMenu()
    {
        PlayerManager playerManager = GameManager.Instance.playerManager;
        //Gets Objects
        GameObject blackScreen = playerManager.darkenScreen;
        //CardDetails cardDisplay = playerManager.choiceButtonCardDisplay.GetComponent<CardDetails>();
        //RectTransform buttonPos = playerManager.choiceButtonsPos;
        //GameObject choiceButtonsParent = playerManager.choiceButtonsParent;
        //List<GameObject> choiceButtons = playerManager.choiceButtons;

        //Hide Black screen
        blackScreen.SetActive(false);
    }

}