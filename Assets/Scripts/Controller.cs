using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject scoreManager;
    public GameObject player;
    public void QuitGame()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
    
    public void LoadLevel(string levelName) 
    {
        SceneManager.LoadScene(levelName);
    }
    
    // When the user actively *presses* the complete mission button
    public void GameOver() 
    {
        scoreManager = GameObject.Find("ScoreManager");
        player = GameObject.Find("Player");
        int finalScore = scoreManager.GetComponent<ScoreManager>().score;
        int finalKills = scoreManager.GetComponent<ScoreManager>().kills;
        int finalExperience = (int) player.GetComponent<PlayerController>().experience;
        PlayerPrefs.SetInt("score", finalScore);
        PlayerPrefs.SetString("surived", "Survived");
        PlayerPrefs.SetInt("kills", finalKills);
        PlayerPrefs.SetInt("totalExp", finalExperience);
        SceneManager.LoadScene("GameOver");
    }
}
