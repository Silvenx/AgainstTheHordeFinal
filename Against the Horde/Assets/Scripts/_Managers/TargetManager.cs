using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance;

    private Target currentTarget;
    private GameObject thisCard;
    private System.Action<GameObject[]> onTargetsSelected;

    private List<GameObject> potentialTargets = new List<GameObject>();
    private List<GameObject> selectedTargets = new List<GameObject>();
    private int numberOfTargets;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of TargetManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void StartTargetSelection(Target target, GameObject thisCard, System.Action<GameObject[]> callback)
    {
        this.currentTarget = target;
        this.thisCard = thisCard;
        this.onTargetsSelected = callback;

        selectedTargets.Clear();
        potentialTargets.Clear();

        // Get potential targets from the target
        potentialTargets.AddRange(target.GetPotentialTargets());

        numberOfTargets = target.GetNumberOfTargets();

        // Start the selection process
        StartCoroutine(HandlePlayerSelection());
    }

    private IEnumerator HandlePlayerSelection()
    {
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
                yield return null; // Wait for the next frame
            }
            if (clickedTarget != null)
            {
                selectedTargets.Add(clickedTarget);
                Debug.Log("Selected Target: " + clickedTarget.name);
            }
        }

        // Selection complete, call the callback
        if (onTargetsSelected != null)
        {
            onTargetsSelected(selectedTargets.ToArray());
        }
    }

    private bool IsPointerOverUIObject()
    {
        // Check if the mouse is over a UI element to prevent accidental clicks
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
