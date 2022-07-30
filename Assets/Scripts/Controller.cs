using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayGame");
    }
    
    public void Configure()
    {
        SceneManager.LoadScene("Configure");
    }
    
    public void MainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
