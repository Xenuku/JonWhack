using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public bool enableSpawn=false;
    public GameObject Enemy;
    public GameObject[] enemies;


    public float minx; //player position x +30
    public float maxx;// player position x+50

    public float miny; // player position x+30
    public float maxy;// player position y+50

    public float waitingForNextSpawn=10;
    public float theCountdown=10;
    
    
    protected Transform playerTransform;
    private GameObject objPlayer;


    // Start is called before the first frame update
    void Start()
    {
        objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;
        //InvokeRepeating("SpawnEnemy", 3, 1); // after 3sec do this everysec;
        Debug.Log(GameObject.FindWithTag("Player").transform.position);
        playerTransform.SendMessage("Test",(int) 1106);

    }

    // Update is called once per frame
    void Update()
    {   
                //send a message to player side
                playerTransform.SendMessage("Test",(int) 1106);
                Debug.Log(GameObject.FindWithTag("Player").transform.position);
                //theCountdown-=Time.deltaTime;

    }




    // void SpawnEnemy(){
    //     float randomX=Random.Range(0.5f,0.5f);
    //     if(enableSpawn){
    //         enemies=GameObject.FindWithTag("melee_enemy");
    //         if(enemies.Length<20){
    //             GameObject enemy=(GameObject)Instantiate(Enemy,new Vector(randomX,0.5f,0f),Quaternion.identity);

    //         }
    //     }
    // }
}
