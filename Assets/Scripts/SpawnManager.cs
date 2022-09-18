using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool enableSpawn=true;
    public GameObject Enemy;

    public int enemyNum;

    public GameObject [] EnemyTypes;
    public Transform[] spawnPoint;

    public int respawnTime;
    public int currentTime;
    public int increaseRespawn;
    public int reset;

    
    // Start is called before the first frame update
    void Start()
    {   
        increaseRespawn=30;
        enemyNum=1;
        currentTime=0;
        respawnTime=15; //every 3 seconds
       
        //Debug.Log(ranEnemy);

        //InvokeRepeating("SpawnEnemy", 3, 1); // after 3sec do this everysec;
    }

    // Update is called once per frame
    void Update()
    {
        //curDelay+=Time.deltaTime;
        //currentTime+=(int) Time.deltaTime;
        InvokeRepeating("AddSecond",20,5);
        InvokeRepeating("SpawnSystem",5,5);
        InvokeRepeating("manageEnemyNum",5,5);

        if(respawnTime<2){
            CancelInvoke("SpawnSystem");
        }
        

    }
    void SpawnEnemy(){
        //Debug.Log(Camera.main.transform.position.x);
        //Debug.Log(Camera.main.transform.position.y);

        int ranEnemy = Random.Range(0, EnemyTypes.Length);
        float randomX = Random.Range(Camera.main.transform.position.x-15.0f, Camera.main.transform.position.x+15.0f); 
        float randomY =Random.Range(Camera.main.transform.position.y-15.0f, Camera.main.transform.position.y+15.0f); 
        if (enableSpawn){
            GameObject enemy=(GameObject)Instantiate(Enemy, new Vector2(randomX, randomY), Quaternion.identity);
            enemyNum+=1;
        }
    }
    void AddSecond(){
        currentTime+=1;

    }
    void SpawnSystem(){
        //Debug.Log(currentTime);
        //Debug.Log(currentTime&respawnTime);
        if(Mathf.Floor(currentTime&respawnTime)==0){ //for respawn time
           SpawnEnemy();
            //SpawnEnemy1();
        }
        if(Mathf.Floor(currentTime&increaseRespawn)==0){
            respawnTime-=1;
            //Debug.Log(respawnTime);
        }
        if(Mathf.Floor(currentTime&reset)==0){
            respawnTime=15;
            //Debug.Log(respawnTime);
        }
  
    }
    void manageEnemyNum() {
      //Debug.Log(Mathf.Floor(currentTime&increaseRespawn)==0);
        
        if(enemyNum>14){
            enableSpawn=false;

        }
        else if(enemyNum<=15){
            enableSpawn=true;
        }
        //Debug.Log(enemyNum);
    }
    
}
