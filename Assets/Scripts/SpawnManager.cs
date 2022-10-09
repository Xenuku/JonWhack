using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public bool enableSpawn = true;
    public int maxEnemyNum;
    public int curEnemyNum;
    public int maxEliteNum;
    public int curEliteNum;
    public int maxCaptainNum;
    public int curCaptainNum;

    public int addNormal;
    public int addElite;
    public int addCaptain;

    public AudioSource musicPlayer;
    public List<AudioClip> audios;

    public GameObject[] EnemyTypes;
    public GameObject[] EliteEnemyTypes;
    public GameObject Captain;

    private float timeElapsed = 0.0f;
    private int playerLevel;
    private float playerDistance;
    private Vector2 spawnPosition;

    //references
    public GameObject player;

    public TMP_Text difficultyText;

    public enum State
    {
        difficulty1,
        difficulty2,
        difficulty3,
        boss,
    }

    public State curState;

    void Start()
    {
        curState = State.difficulty1;

        curEliteNum = 0;
        curEnemyNum = 0;

        playerLevel = player.GetComponent<PlayerController>().level;

        musicPlayer.clip = audios[0];
        musicPlayer.Play();
    }


    void Update()
    {
        timeElapsed += Time.deltaTime;

        switch (curState)
        {
            case State.difficulty1: Difficulty1(); break;
            case State.difficulty2: Difficulty2(); break;
            case State.difficulty3: Difficulty3(); break;
            case State.boss: Boss(); break;
        }

        if (playerLevel >= 2 && playerLevel < 4)
        {
            curState = State.difficulty2;
        }
        else if (playerLevel >= 4 && playerLevel < 7)
        {
            curState = State.difficulty3;
        }
        else if (playerLevel >= 7)
        {
            curState = State.boss;
        }
    }

    protected void Difficulty1()
    {
        addNormal = 0;
        addElite = 0;
        difficultyText.text = "Threat Level: <color=\"green\">Low</color>";
        SpawnEnemy();
    }

    protected void Difficulty2()
    {
        addNormal = 10;
        addElite = 3;
        difficultyText.text = "Threat Level: <color=\"orange\">Medium</color>";
        SpawnEnemy();
    }

    private bool bossTheme = false;
    protected void Difficulty3()
    {
        if(bossTheme == false)
        {
            musicPlayer.clip = audios[1];
            musicPlayer.Play();
            bossTheme = true;
        }

        addNormal = 20;
        addElite = 6;
        difficultyText.text = "Threat Level: <color=\"red\">High</color>";
        SpawnEnemy();
        SpawnCaptain();
    }

    protected void Boss()
    {
        addNormal = 30;
        addElite = 9;
        difficultyText.text = "Threat Level: <color=#2e293a>Midnight</color>";
        SpawnEnemy();
        SpawnCaptain();
    }

    void SpawnEnemy()
    {
        int ranElite = Random.Range(0, EliteEnemyTypes.Length);
        int ranEnemy = Random.Range(0, EnemyTypes.Length);

        spawnPosition.x = Random.Range(Camera.main.transform.position.x - 50.0f, Camera.main.transform.position.x + 50.0f);
        spawnPosition.y = Random.Range(Camera.main.transform.position.y - 50.0f, Camera.main.transform.position.y + 50.0f);

        playerDistance = Vector2.Distance(spawnPosition, player.transform.position);

        while (playerDistance <= 30.0f)
        {
            spawnPosition.x = Random.Range(Camera.main.transform.position.x - 50.0f, Camera.main.transform.position.x + 50.0f);
            spawnPosition.y = Random.Range(Camera.main.transform.position.y - 50.0f, Camera.main.transform.position.y + 50.0f);
            playerDistance = Vector2.Distance(spawnPosition, player.transform.position);
        }

        if (curEnemyNum < maxEnemyNum + addNormal)
        {
            GameObject enmeies = (GameObject)Instantiate(EnemyTypes[ranEnemy], spawnPosition, Quaternion.identity);

            curEnemyNum += 1;
        }

        if (curEliteNum < maxEliteNum + addElite)
        {
            GameObject Elites = (GameObject)Instantiate(EliteEnemyTypes[ranElite], spawnPosition, Quaternion.identity);
            
            curEliteNum += 1;
        }
        
    }

    void SpawnCaptain()
    {
        int ranElite = Random.Range(0, EliteEnemyTypes.Length);
        int ranEnemy = Random.Range(0, EnemyTypes.Length);

        spawnPosition.x = Random.Range(Camera.main.transform.position.x - 50.0f, Camera.main.transform.position.x + 50.0f);
        spawnPosition.y = Random.Range(Camera.main.transform.position.y - 50.0f, Camera.main.transform.position.y + 50.0f);

        playerDistance = Vector2.Distance(spawnPosition, player.transform.position);

        while (playerDistance <= 30.0f)
        {
            spawnPosition.x = Random.Range(Camera.main.transform.position.x - 50.0f, Camera.main.transform.position.x + 50.0f);
            spawnPosition.y = Random.Range(Camera.main.transform.position.y - 50.0f, Camera.main.transform.position.y + 50.0f);
            playerDistance = Vector2.Distance(spawnPosition, player.transform.position);
        }

        if (curCaptainNum < maxCaptainNum + addCaptain)
        {
            GameObject BadAss = (GameObject)Instantiate(Captain, spawnPosition, Quaternion.identity);

            curCaptainNum += 1;
        }
    }

}



