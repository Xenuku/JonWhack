using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public GameObject[] EnemyTypes;
    public GameObject[] EliteEnemyTypes;
    public GameObject Captain;

    private float timeElapsed = 0.0f;
    private int playerLevels;
    private float playerDistance;
    private Vector2 spawnPosition;

    //references
    public GameObject player;

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

        playerLevels = player.GetComponent<PlayerController>().level;
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

        if (playerLevels >= 5 && playerLevels < 10)
        {
            curState = State.difficulty2;
        }
        else if (playerLevels >= 10 && playerLevels < 15)
        {
            curState = State.difficulty3;
        }
        else if (playerLevels >= 15)
        {
            curState = State.boss;
        }
    }

    protected void Difficulty1()
    {
        addNormal = 0;
        addElite = 0;

        SpawnEnemy();
    }

    protected void Difficulty2()
    {
        addNormal = 10;
        addElite = 3;

        SpawnEnemy();
    }

    protected void Difficulty3()
    {
        addNormal = 20;
        addElite = 6;

        SpawnEnemy();
        SpawnCaptain();
    }

    protected void Boss()
    {
        addNormal = 30;
        addElite = 9;

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



