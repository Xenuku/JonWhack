using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    public int health = 200;
    public SpriteRenderer sprite;
    private Transform center;
    private Transform target;
    private float timeElapsed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= 10.0f)
        {
            target = GameObject.FindWithTag("Support").transform;
            target.GetComponent<Support>().enchantLooking = true;
            target.GetComponent<Support>().centerTransform = transform;
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
