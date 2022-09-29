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
    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;
        pauseMenu.SetActive(gameIsPaused);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        gameIsPaused = !gameIsPaused;
        pauseMenu.SetActive(gameIsPaused);
        Time.timeScale = 1;
    }

    public void QuitSession() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
