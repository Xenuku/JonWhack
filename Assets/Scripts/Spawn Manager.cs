using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool enableSpawn=false;
    public GameObject Enemy;
    void SpawnEnemy(){
        float randomX = Random.Range(-0.5f, 0.5f); 
        if (enableSpawn){
            GameObject enemy=(GameObject)Instantiate(Enemy, new Vector3(randomX, 1.1f, 0f), Quaternion.identity);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 3, 1); // after 3sec do this everysec;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
