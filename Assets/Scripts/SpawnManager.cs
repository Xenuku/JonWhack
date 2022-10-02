using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool enableSpawn = true;
    public GameObject Enemy;

    public int enemyNum;

    public GameObject[] EnemyTypes;
    public GameObject[] BigEnemyTypes;

    public Transform[] spawnPoint;

    public int respawnTime;
    public int currentTime;
    public int increaseRespawn;
    public int reset;
    public int enemy_num_limit;
    //public randomX_1;


    // Start is called before the first frame update
    void Start()
    {
        enemy_num_limit = 14;

        increaseRespawn = 30;
        enemyNum = 1;
        currentTime = 0;
        respawnTime = 15; //every 3 seconds
        InvokeRepeating("AddSecond", 20f, 1f);
        InvokeRepeating("SpawnSystem", 1f, 0.5f);
        //InvokeRepeating("manageEnemyNum",5f,1f);
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        manageEnemyNum();
    }
    void SpawnEnemy()
    {


        int ranEnemy = Random.Range(0, EnemyTypes.Length);

        int boss=Random.Range(0,10);


        float randomX = Random.Range(Random.Range(Camera.main.transform.position.x - 50.0f, Camera.main.transform.position.x - 40.0f), Random.Range(Camera.main.transform.position.x + 50.0f, Camera.main.transform.position.x + 40.0f));



        //float randomY =Random.Range(Camera.main.transform.position.y-15.0f, Camera.main.transform.position.y+15.0f); 

        float randomY = Random.Range(Random.Range(Camera.main.transform.position.y - 50.0f, Camera.main.transform.position.y - 40.0f), Random.Range(Camera.main.transform.position.y + 50.0f, Camera.main.transform.position.y + 40.0f));


        if (enableSpawn)
        {
            GameObject enemy = (GameObject)Instantiate(EnemyTypes[ranEnemy], new Vector2(randomX, randomY), Quaternion.identity);
            enemyNum += 1;
        }
        int ranBigEnemy= Random.Range(0,BigEnemyTypes.Length);
        Debug.Log(boss);

        if(enableSpawn &boss<3){
            GameObject BigEnemy=    (GameObject)Instantiate(BigEnemyTypes[ranBigEnemy], new Vector2(randomX, randomY), Quaternion.identity);
        }
    }
    void AddSecond()
    {
        currentTime += 1;
    }
    void SpawnSystem()
    {
        //Debug.Log(currentTime);
        //Debug.Log(currentTime&respawnTime);
        if (Mathf.Floor(currentTime & respawnTime) == 0)
        { //for respawn time
            SpawnEnemy();
            //Debug.Log("spawnenemy working");
        }
        if (Mathf.Floor(currentTime & increaseRespawn) == 0)
        {
            respawnTime -= 1;
            enemy_num_limit += 1;
            //Debug.Log(respawnTime);
        }
        if (Mathf.Floor(currentTime & reset) == 0)
        {
            respawnTime = 15;
            //Debug.Log(respawnTime);
        }

    }
    void manageEnemyNum()
    {
        //Debug.Log(Mathf.Floor(currentTime&increaseRespawn)==0);

        if (enemyNum > enemy_num_limit)
        {
            enableSpawn = false;

        }
        else if (enemyNum <= enemy_num_limit + 1)
        {
            enableSpawn = true;
        }
        //Debug.Log(enemyNum);
    }
    void reduceEnemy(int die)
    {
        enemyNum -= die;
    }

}
