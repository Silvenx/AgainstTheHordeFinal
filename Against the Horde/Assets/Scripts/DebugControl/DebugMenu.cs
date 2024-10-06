using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    public PlayerManager playerManager;
    public GameManager gameManager;
    public GameObject debugMenu;

    public void DebugToggleDebugMenu()
    {
        debugMenu.SetActive(!debugMenu.activeSelf);
        Debug.Log("Debug Menu - Debug Menu Toggled");
    }

    public void DebugRestartGame()
    {
        Debug.Log("Debug Menu - Restarting Game");
        Destroy(GameManager.Instance.gameObject);  // Destroy the current GameManager instance
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DebugDrawCard()
    {
        Debug.Log("Debug Menu - Drawing Card");
        playerManager.DrawCardFromTopOfDeck();
    }

    public void DebugSetEnergyHigh()
    {
        int energyToSet = 55;
        gameManager.SetCurrentEnergy(energyToSet);
        gameManager.SetMaxEnergy(energyToSet, true);
    }

}

