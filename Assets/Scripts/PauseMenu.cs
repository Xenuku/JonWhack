using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;
    public GameObject pauseMenu;

    void Update()
    {
        // If the player presses escape, display the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameIsPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }
    // Pause time, show the menu and set the gameIsPaused to false
    public void PauseGame()
    {
        Time.timeScale = 0;
        gameIsPaused = !gameIsPaused;
        pauseMenu.SetActive(gameIsPaused);
    }
    // Resume time, hide the menu and set gameisPause to true (opposite of current value)
    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameIsPaused = !gameIsPaused;
        pauseMenu.SetActive(gameIsPaused);
    }
    // If the user presses the quit button, go back to main menu
    public void QuitSession() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
