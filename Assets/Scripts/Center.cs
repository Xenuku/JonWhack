using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    public int health = 200;
    public SpriteRenderer sprite;
    private Transform center;
    private Transform target;
    private int targetType;
    private float timeElapsed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        targetType = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= 10.0f && targetType == 1 && GameObject.FindWithTag("Melee") != null)
        {
            target = GameObject.FindWithTag("Melee").transform;
            target.GetComponent<MeleeEnemy>().enchantLooking = true;
            target.GetComponent<MeleeEnemy>().centerTransform = transform.position;
        }
        else if (timeElapsed >= 10.0f && targetType == 2 && GameObject.FindWithTag("Sniper") != null)
        {
            target = GameObject.FindWithTag("Sniper").transform;
            target.GetComponent<Sniper>().enchantLooking = true;
            target.GetComponent<Sniper>().centerTransform = transform.position;
        }
        else if (timeElapsed >= 10.0f && targetType == 3 && GameObject.FindWithTag("Support") != null)
        {
            target = GameObject.FindWithTag("Support").transform;
            target.GetComponent<Support>().enchantLooking = true;
            target.GetComponent<Support>().centerTransform = transform.position;
        }
        else if (timeElapsed >= 10.0f && targetType == 4 && GameObject.FindWithTag("Heavy")!= null)
        {
            target = GameObject.FindWithTag("Heavy").transform;
            target.GetComponent<Heavy>().enchantLooking = true;
            target.GetComponent<Heavy>().centerTransform = transform.position;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
