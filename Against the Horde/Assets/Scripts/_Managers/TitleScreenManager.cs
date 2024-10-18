using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject titleMenu;
    public GameObject factionMenu;
    public void OpenFactionMenu()
    {
        factionMenu.SetActive(true);

        titleMenu.SetActive(false);
    }

    public void OpenTitleMenu()
    {
        titleMenu.SetActive(true);

        factionMenu.SetActive(false);
    }

}
