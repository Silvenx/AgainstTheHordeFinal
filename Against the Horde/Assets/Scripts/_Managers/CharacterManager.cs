using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Managers")]
    public GameManager gameManager;
    public FieldManager fieldManager;
    public float timeForCardsToMove = 1.5f;
    public float cardMoveSpeed = 2f;

    [Header("My Deck")]
    public GameObject deckGameObject;
    public Deck myDeck;
    


    public void setDeck(Deck deck)
    {
        //Populate Deck in game
        myDeck = deck;
    }
    

    protected IEnumerator WaitSeconds(float secToWait)
    {
        Debug.Log("Waiting " + secToWait + " seconds.");
        yield return new WaitForSeconds(secToWait);
    }
    //Lerps object from 1 pos to another with a soft finish (sin graph)
    protected IEnumerator LerpObjectMovement(GameObject objectToMove, Vector3 startPos, Vector3 endPos, float timeToMove)
    {
        float currentLerpTime = 0;
        while (currentLerpTime <= timeToMove) //until X sec passed
        {
            currentLerpTime += Time.deltaTime * cardMoveSpeed;
            float perc = currentLerpTime / timeToMove;
            perc = Mathf.Sin(perc * Mathf.PI * 0.5f);
            objectToMove.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(startPos, endPos, perc);

            //End early if reached position
            if (Vector3.Distance(objectToMove.transform.position, endPos) <= 0.01f)
            {
                objectToMove.GetComponent<CardDetails>().currentCoroutine = null;
                break;
            }

            yield return 1; //wait for next frame
        }
        objectToMove.GetComponent<CardDetails>().currentCoroutine = null;
    }

}
