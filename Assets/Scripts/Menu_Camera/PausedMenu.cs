using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PausedMenu : MonoBehaviour
{
    public GameObject Container;
    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) ResumeButton();
            else PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Container.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeButton()
    {
        isPaused = false;
        Container.SetActive(false);
        Time.timeScale = 1f;
       // Debug.Log("Resume button clicked!");

    }

    public void RestartButton()
    {
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        
       // Debug.Log("Restart button clicked!");

        
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGameButton()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}