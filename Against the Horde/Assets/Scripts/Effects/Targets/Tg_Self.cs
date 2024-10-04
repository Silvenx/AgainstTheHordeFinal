using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Target/Target_Self")]
public class Tg_Self : Target
{

    public override IEnumerator TargetAquisition(GameObject thisCard = null)
    {
        finalList.Clear();
        // Start the coroutine to handle player selection and wait for it to complete
        yield return GameManager.Instance.StartCoroutine(GetMyTargets(thisCard));
    }

    // Method to retrieve selected targets after selection is complete
    public override GameObject[] getTargets()
    {
        return finalList.ToArray();
    }

    //--------------------------------------Targeting Logic--------------------------------------//

    public IEnumerator GetMyTargets(GameObject thisCard = null)
    {
        finalList.Add(thisCard);
        yield return null;
    }
}
