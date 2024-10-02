using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_ManualSelect")]
public class Tg_ManualSelect : Target

{
    public FieldToTarget targetGroup;
    public int numberOfTargets = 1; //default to 1 just in case

    public enum FieldToTarget
    {
        ALL_MONSTERS,
        PLAYER_MONSTERS,
        HORDE_MONSTERS
    }

    private List<GameObject> selectedTargets = new List<GameObject>();

    public override GameObject[] getTargets(GameObject thisCard = null)
    {
        HandlePlayerSelection(thisCard);
        Debug.Log("WIN:Targets selected" + selectedTargets);
        return selectedTargets.ToArray();
    }

    public void HandlePlayerSelection(GameObject thisCard)
    {
        FieldManager fieldManager = GameManager.Instance.fieldManager;

        List<GameObject> potentialTargets = new List<GameObject>();

        switch (targetGroup)
        {
            //Adds the potential targets to the list called potential targets
            case FieldToTarget.PLAYER_MONSTERS:
                potentialTargets.AddRange(fieldManager.getAllPlayerMonsters());
                break;

            case FieldToTarget.HORDE_MONSTERS:
                potentialTargets.AddRange(fieldManager.getAllHordeMonsters());
                break;

            case FieldToTarget.ALL_MONSTERS:
                potentialTargets.AddRange(fieldManager.getAllPlayerMonsters());
                potentialTargets.AddRange(fieldManager.getAllHordeMonsters());
                break;
        }

        if (potentialTargets.Count == 0)
        {
            Debug.Log("No targets to select.");

        }

        //FUTURE: Add in highlight targets here

        List<GameObject> selectedTargets = new List<GameObject>();

        while (selectedTargets.Count < numberOfTargets)
        {
            // Wait for player input
            GameObject clickedTarget = null;
            bool inputReceived = false;

            // Wait until the player clicks on a valid target
            while (!inputReceived)
            {
                if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        GameObject hitObject = hit.collider.gameObject;

                        if (potentialTargets.Contains(hitObject) && !selectedTargets.Contains(hitObject))
                        {
                            clickedTarget = hitObject;
                            inputReceived = true;
                        }
                    }
                }

            }
            if (clickedTarget != null)
            {
                selectedTargets.Add(clickedTarget);
                Debug.Log("Selected Target: " + clickedTarget.name);
            }
        }

    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}