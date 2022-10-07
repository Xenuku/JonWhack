using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool enableSpawn = true;

    public int enemyNum = 1;

    public GameObject[] EnemyTypes;
    public GameObject[] BigEnemyTypes;

    public Transform[] spawnPoint;

    public int respawnTime = 5; 
    private int currentTime;
    public int increaseRespawn = 5;
    //public int reset;
    public int enemy_num_limit = 15;
    //public randomX_1;


    // Start is called before the first frame update
    void Start()
    {
        respawnTime=1; //first spawn
        increaseRespawn=5;
        enemy_num_limit=30;

        currentTime = 0;
        InvokeRepeating("AddSecond", 0, 0.5f);
        //InvokeRepeating("SpawnSystem", 0, 0.1f);
        
        //InvokeRepeating("manageEnemyNum",5f,1f);
        //SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnSystem();
        
        manageEnemyNum();
        //SpawnSystem();
        

    }
    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, EnemyTypes.Length);
        int boss = Random.Range(0, 10);
        float randomX = Random.Range(Random.Range(Camera.main.transform.position.x - 50.0f, Camera.main.transform.position.x - 40.0f), Random.Range(Camera.main.transform.position.x + 50.0f, Camera.main.transform.position.x + 40.0f));
        float randomY = Random.Range(Random.Range(Camera.main.transform.position.y - 50.0f, Camera.main.transform.position.y - 40.0f), Random.Range(Camera.main.transform.position.y + 50.0f, Camera.main.transform.position.y + 40.0f));
        // if (enableSpawn)
        // {
        //     GameObject enemy = (GameObject)Instantiate(EnemyTypes[ranEnemy], new Vector2(randomX, randomY), Quaternion.identity);
        //     enemyNum += 1;
        // }
        int ranBigEnemy = Random.Range(0, BigEnemyTypes.Length);
        if(enableSpawn){
            if ( boss < 3 )
            {
                GameObject BigEnemy = (GameObject)Instantiate(BigEnemyTypes[ranBigEnemy], new Vector2(randomX, randomY), Quaternion.identity);
                enemyNum+=1;
                
            }else{
                GameObject enemy = (GameObject)Instantiate(EnemyTypes[ranEnemy], new Vector2(randomX, randomY), Quaternion.identity);
                enemyNum += 1;
            }
        }
        Debug.Log(enemyNum);

    }
    void AddSecond()
    {
        currentTime += 1;

        //Debug.Log(enemy_num_limit);


    }
    void SpawnSystem()
    {
        if (Mathf.Floor(currentTime % respawnTime) == 0)
        { //for respawn time
            //Debug.Log("spawn enemy");
            SpawnEnemy();
        }
        if (Mathf.Floor(currentTime % increaseRespawn) == 0)
        {   

            if(respawnTime<2){
            //Debug.Log("increase num enemy");

            enemy_num_limit += 1;
            respawnTime=7; //from second respawn
            //Debug.Log(enemy_num_limit);

            }else{
            //Debug.Log("decrease respawn time");

            respawnTime -= 1;
           // Debug.Log(respawnTime);


            }
        }
        
    }
    void manageEnemyNum()
    {
       
        if (enemyNum > enemy_num_limit)
        {
            enableSpawn = false;
        }
        else if (enemyNum <= enemy_num_limit + 1)
        {
            enableSpawn = true;
        }
    }
    void reduceEnemy(int die)
    {
        //Debug.Log("reduceEnemy ");
        enemyNum -= die;
    }

}
