using System;
using JetBrains.Annotations;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PausedMenu : MonoBehaviour
{

    public GameObject Container;
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Container.SetActive(true);
            Time.timeScale = 0;

        }
    }

    public void ResumeButton()
    {
        Container.SetActive(false);
        Time.timeScale = 1;

    }
    
    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ponovo učitava trenutnu scenu koju dobiva iz GetActiveScene
    }

    public void MainMenuButton()
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // KADA SE NAPRAVI MAIN MENU OVJDE IDE DA SE UČITA!
    }

    public void OptionsButton()
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Options"); // KADA SE NAPRAVI OPTIONS MENU OVJDE IDE DA SE UČITA!
    }
    
    public void QuitGameButton()
    {
        Application.Quit();
    }
}
