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
        print("Time" + Time.timeScale);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        gameIsPaused = !gameIsPaused;
        pauseMenu.SetActive(gameIsPaused);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameIsPaused = !gameIsPaused;
        pauseMenu.SetActive(gameIsPaused);
    }

    public void QuitSession() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
