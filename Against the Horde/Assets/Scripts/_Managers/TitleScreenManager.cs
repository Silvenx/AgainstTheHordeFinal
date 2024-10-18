using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public List<GameObject> menuList;

    public void OpenMenu(GameObject menuToOpen)
    {
        foreach (GameObject menu in menuList)
        {
            menu.SetActive(menu == menuToOpen);
        }
    }

    public void OpenFactionMenu()
    {
        OpenMenu(menuList.Find(menu => menu.name == "Faction_Menu"));
    }

    public void OpenTitleMenu()
    {
        OpenMenu(menuList.Find(menu => menu.name == "Title_Menu"));
    }

    public void OpenLegionMenu()
    {
        OpenMenu(menuList.Find(menu => menu.name == "Legion_Menu"));
    }

}
