using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public bool enableSpawn=false;
    public GameObject Enemy;
    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("SpawnEnemy", 3, 1); // after 3sec do this everysec;
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
