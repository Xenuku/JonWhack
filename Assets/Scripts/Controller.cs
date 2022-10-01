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
    
    public void LoadLevel(string levelName) 
    {
        SceneManager.LoadScene(levelName);
    }
    
}
