using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public bool enableSpawn = true;
    public int maxEnemyNum; //set the max Enemy number for limitation
    public int curEnemyNum; // current spawned enemy number
    public int maxEliteNum; // limit of elite number
    public int curEliteNum; // current elite number
    public int maxCaptainNum; // limit of captain enemy type number
    public int curCaptainNum; // current number


    public int addNormal; // add normal enemy type limit.
    public int addElite; // add elite enemy type limit.
    public int addCaptain; //add captain enemy type limit.
    //music player
    public AudioSource musicPlayer;
    public List<AudioClip> audios;

    //enemy lists
    public GameObject[] EnemyTypes; // list of enemy type
    public GameObject[] EliteEnemyTypes; // list of elite enemy type
    public GameObject Captain; // Boss

    //some references variable
    private float timeElapsed = 0.0f; // for the setting a time
    private int playerLevel; // get player level for threat level
    private float playerDistance; //distance between user and enemy
    private Vector2 spawnPosition; //enemy postion

    //references
    public GameObject player;

    public TMP_Text difficultyText;

    //4 difficulty according to player level
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
        //on start, initial level is difficulty1, loop index 0 music in the list
        curState = State.difficulty1;

        curEliteNum = 0;
        curEnemyNum = 0;

        musicPlayer.clip = audios[0];
        musicPlayer.Play();
    }


    void Update()
    {
        timeElapsed += Time.deltaTime;

        playerLevel = player.GetComponent<PlayerController>().level;


        switch (curState)
        {
            case State.difficulty1: Difficulty1(); break;
            case State.difficulty2: Difficulty2(); break;
            case State.difficulty3: Difficulty3(); break;
            case State.boss: Boss(); break;
        }

        //difficulty1: level 1, difficulty2: level 2-4
        //difficulty3: level 4-7, boss: level >= 7
        if (playerLevel >= 2 && playerLevel < 4)
        {
            curState = State.difficulty2; // during player level 2 to 3 difficulty 2
        }
        else if (playerLevel >= 4 && playerLevel < 7) // during player level 4 to 6 difficulty 3
        {
            curState = State.difficulty3;
        }
        else if (playerLevel >= 7)
        {
            curState = State.boss; //generate boss
        }
    }

    protected void Difficulty1()
    {
        //no add-on enemies on first difficulty
        addNormal = 0;
        addElite = 0;
        difficultyText.text = "Threat Level: <color=\"green\">Low</color>";
        SpawnEnemy(); //spawn enemy and present difficultyText for playgame scene.
    }

    //added more enemy for 2nd difficulty
    protected void Difficulty2()
    {
        addNormal = 10;
        addElite = 3;
        difficultyText.text = "Threat Level: <color=\"orange\">Medium</color>";
        SpawnEnemy();
    }

    //setup bool for prevent repeating setup musics
    private bool bossTheme = false;

    //new music for difficulty3
    protected void Difficulty3()
    {
        if(bossTheme == false)
        {
            musicPlayer.clip = audios[1];
            musicPlayer.Play();
            bossTheme = true; //for before boss entry
        }

        addNormal = 20;
        addElite = 6;
        difficultyText.text = "Threat Level: <color=\"red\">High</color>";
        SpawnEnemy();
        //spawn captain start from this difficulty
        SpawnCaptain();
    }

    protected void Boss()
    {
        addNormal = 30;
        addElite = 9;
        difficultyText.text = "Threat Level: <color=#2e293a>Midnight</color>";
        SpawnEnemy();
        SpawnCaptain(); //go into boss phase
    }

    void SpawnEnemy()
    {
        //random enemy selected each generation
        int ranElite = Random.Range(0, EliteEnemyTypes.Length);
        int ranEnemy = Random.Range(0, EnemyTypes.Length);

        //random spawn position within 50.0f from center of camera
        spawnPosition.x = Random.Range(Camera.main.transform.position.x - 50.0f, Camera.main.transform.position.x + 50.0f);//  outside of user main camera
        spawnPosition.y = Random.Range(Camera.main.transform.position.y - 50.0f, Camera.main.transform.position.y + 50.0f);

        //update player distance for future use
        playerDistance = Vector2.Distance(spawnPosition, player.transform.position);

        //re-generate if distance to close to player
        while (playerDistance <= 30.0f)
        {
            spawnPosition.x = Random.Range(Camera.main.transform.position.x - 50.0f, Camera.main.transform.position.x + 50.0f);
            spawnPosition.y = Random.Range(Camera.main.transform.position.y - 50.0f, Camera.main.transform.position.y + 50.0f);
            playerDistance = Vector2.Distance(spawnPosition, player.transform.position);
        }

        //if currrent enemy number is lower than maximum, generat more enemy until maximum
        if (curEnemyNum < maxEnemyNum + addNormal)
        {
            GameObject enmeies = (GameObject)Instantiate(EnemyTypes[ranEnemy], spawnPosition, Quaternion.identity);

            curEnemyNum += 1; // if current enemy is less than limit then respawn enemy
        }

        if (curEliteNum < maxEliteNum + addElite)
        {
            GameObject Elites = (GameObject)Instantiate(EliteEnemyTypes[ranElite], spawnPosition, Quaternion.identity);
            
            curEliteNum += 1; //if elite enemy is less than limit then respawn elite.
        }
        
    }

    void SpawnCaptain()
    {
        int ranElite = Random.Range(0, EliteEnemyTypes.Length);
        int ranEnemy = Random.Range(0, EnemyTypes.Length);

        spawnPosition.x = Random.Range(Camera.main.transform.position.x - 50.0f, Camera.main.transform.position.x + 50.0f);
        spawnPosition.y = Random.Range(Camera.main.transform.position.y - 50.0f, Camera.main.transform.position.y + 50.0f);//location setting

        playerDistance = Vector2.Distance(spawnPosition, player.transform.position);

        while (playerDistance <= 30.0f)
        {
            spawnPosition.x = Random.Range(Camera.main.transform.position.x - 50.0f, Camera.main.transform.position.x + 50.0f);
            spawnPosition.y = Random.Range(Camera.main.transform.position.y - 50.0f, Camera.main.transform.position.y + 50.0f);// not respawn near the player
            playerDistance = Vector2.Distance(spawnPosition, player.transform.position);
        }

        if (curCaptainNum < maxCaptainNum + addCaptain)
        {
            GameObject BadAss = (GameObject)Instantiate(Captain, spawnPosition, Quaternion.identity);

            curCaptainNum += 1;
        }
    }

}



