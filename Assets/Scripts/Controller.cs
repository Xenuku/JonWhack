using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject scoreManager;
    public GameObject player;
    public GameObject helpPage1;
    public GameObject helpPage2;

    public void QuitGame()
    {
        Application.Quit(); // Close the game
    }
    // Used by the main menu mostly, to load to the scene required
    public void LoadLevel(string levelName) 
    {
        SceneManager.LoadScene(levelName);
    }
    // Show the help page, if the user clicks to the next page
    // hide the first page and show the next, if the user closes, set both
    // to inactive to hide both
    public void ShowHelp(string page) {
        if(page == "Page1") {
            helpPage1.SetActive(true);
            helpPage2.SetActive(false);
        } else if (page == "Page2") {
            helpPage1.SetActive(false);
            helpPage2.SetActive(true);
        } else {
            helpPage1.SetActive(false);
            helpPage2.SetActive(false);
        }
    }

    // Store player stats for the gameover screen
    public void GameOver(string survived) 
    {
        scoreManager = GameObject.Find("ScoreManager");
        player = GameObject.Find("Player");
        int finalScore = scoreManager.GetComponent<ScoreManager>().score;
        int finalKills = scoreManager.GetComponent<ScoreManager>().kills;
        int finalExperience = (int) player.GetComponent<PlayerController>().experience;
        PlayerPrefs.SetInt("score", finalScore);
        PlayerPrefs.SetString("survived", survived);
        PlayerPrefs.SetInt("kills", finalKills);
        PlayerPrefs.SetInt("totalExp", finalExperience);
        SceneManager.LoadScene("GameOver");
    }
}
