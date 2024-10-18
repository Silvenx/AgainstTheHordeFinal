using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadCombatScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
