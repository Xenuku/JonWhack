using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public int kills = 0;
    public TMP_Text scoreText;
    public GameObject finishGame;
    public Button finishGameBtn;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        // When the game starts, wipe all data from playerprefs
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetString("surived", "");
        PlayerPrefs.SetInt("kills", 0);
        PlayerPrefs.SetInt("totalExp", 0);
        scoreText.text = "Score: " + score;
    }
    void Awake() 
    {
        StartCoroutine(GetAttention());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        if (score >= 25000)
        {
            finishGame.SetActive(true);
        } else {
            finishGame.SetActive(false);
        }
    }
    // Add to the score the amount from a kill, and increase the kill counter
    public void AddToScore(int amount) 
    {
        score += amount;
        kills += 1;
    }


    // Flash the Finish Game Button so the player is aware they have
    // Reached the required score to 'win' and can end the game without
    // dying if they wish
    private IEnumerator GetAttention()
    {
        ColorBlock cb = finishGameBtn.colors;
        while(true) {
            cb.normalColor = new Color32(0, 255, 50, 255);
            finishGameBtn.colors = cb;
            yield return new WaitForSeconds(1);
            cb.normalColor = new Color32(229, 197, 31, 255);
            finishGameBtn.colors = cb;
            yield return new WaitForSeconds(1);
        }
    }
}
