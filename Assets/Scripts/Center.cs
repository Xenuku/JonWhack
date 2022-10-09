using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{

    public int health;
    public SpriteRenderer sprite;
    private Transform center;
    private Transform target;
    private int targetType;
    //Support center will have cooldown ready upon spawn
    private float timeElapsed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        //randomly pickup a enemytype for each spawn
        targetType = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        //only a target will be enchanted upon selection each 10s
        if (timeElapsed >= 10.0f && targetType == 1 && GameObject.FindWithTag("Melee") != null)
        {
            //target's enchantlooking bool will be turn on and have coordinates of support center
            target = GameObject.FindWithTag("Melee").transform;
            target.GetComponent<MeleeEnemy>().enchantLooking = true;
            target.GetComponent<MeleeEnemy>().centerTransform = transform.position;
            timeElapsed = 0.0f;
        }
        else if (timeElapsed >= 10.0f && targetType == 2 && GameObject.FindWithTag("Sniper") != null)
        {
            target = GameObject.FindWithTag("Sniper").transform;
            target.GetComponent<Sniper>().enchantLooking = true;
            target.GetComponent<Sniper>().centerTransform = transform.position;
            timeElapsed = 0.0f;
        }
        else if (timeElapsed >= 10.0f && targetType == 3 && GameObject.FindWithTag("Support") != null)
        {
            target = GameObject.FindWithTag("Support").transform;
            target.GetComponent<Support>().enchantLooking = true;
            target.GetComponent<Support>().centerTransform = transform.position;
            timeElapsed = 0.0f;
        }
        else if (timeElapsed >= 10.0f && targetType == 4 && GameObject.FindWithTag("Heavy")!= null)
        {
            target = GameObject.FindWithTag("Heavy").transform;
            target.GetComponent<Heavy>().enchantLooking = true;
            target.GetComponent<Heavy>().centerTransform = transform.position;
            timeElapsed = 0.0f;
        }


        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    //flash upon damage
    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
