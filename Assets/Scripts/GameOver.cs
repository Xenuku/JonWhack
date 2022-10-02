using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;
    public TMP_Text winScore;
    public TMP_Text winStats;
    public TMP_Text loseStats;
    public TMP_Text loseScore;
    public int score;
    public string survived;
    public int kills;
    public int totalExp;
    public TMP_InputField playerName;
    public TMP_Text confirmBtnText;

    // Get the score, survived value, kills and experience from storage
    // Display the appropriate screen (win or lose) depending on score obtained
    // If the player won and survived, add an extra 5,000 score
    // If the player has won, they can enter their name into the name field and submit
    // Their score online.
    void Start()
    {
        score = PlayerPrefs.GetInt("score");
        survived = PlayerPrefs.GetString("surived");
        kills = PlayerPrefs.GetInt("kills");
        totalExp = PlayerPrefs.GetInt("totalExp");

        if (score >= 25000) {
            win.SetActive(true);
            lose.SetActive(false);
            //Bonus of 5,000 score for surviving the mission
            if (survived == "Survived") 
            {
                score += 5000;
                winStats.text = "Total Experience: " + totalExp + "\n"
                + "Total Kills: " + kills + "\n"
                + "Survived: <color=\"green\">Yes</color> (+5,000 score)";
            } else {
                winStats.text = "Total Experience: " + totalExp + "\n"
                + "Total Kills: " + kills + "\n"
                + "Survived: <color=\"red\">No</color>";
            }
            winScore.text = "Final Score: " + score.ToString("#,#");
        } else {
            lose.SetActive(true);
            win.SetActive(false);
            loseStats.text = "Total Experience: " + totalExp + "\n"
            + "Total Kills: " + kills + "\n"
            + "Survived: <color=\"red\">No</color>";
            loseScore.text = "Final Score: " + score.ToString("#,#");
        }

    }

    public void SubmitScore() 
    {
        StartCoroutine(submitScoreOnline("http://dreamlo.com/lb/hJYnhROlS0uxMAwxZbomMgVAw0dFGO5EepS_ciW1eflA/"));
    }

    IEnumerator submitScoreOnline(string url) 
    {
        string finalUrl = url + "add/" + playerName.text + "/" + score + "/0/" + survived;
        UnityWebRequest uwr = UnityWebRequest.Get(finalUrl);
        yield return uwr.SendWebRequest();
        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            confirmBtnText.text = "ERROR";
            Debug.Log("Error while getting: " + uwr.error);
        } 
        else
        {
            confirmBtnText.text = "Loading...";
            if(uwr.downloadHandler.text == "OK") 
            {
                SceneManager.LoadScene("HighScores");    
            }
        }
    }
}
