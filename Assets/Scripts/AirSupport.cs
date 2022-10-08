using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AirSupport : MonoBehaviour
{
    private float timeElapsed = 0.0f;
    public float health;

    public UnityEngine.AI.NavMeshAgent enemyAgent;
    private Transform playerTransform;
    public SpriteRenderer sprite;


    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;

        enemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyAgent.updateRotation = false;
        enemyAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        Vector3 bodyScale = transform.localScale;
        if (playerTransform.position.x >= transform.position.x)
        {
            sprite.flipY = false;
        }
        else
        {
            sprite.flipY = true;
        }

        enemyAgent.SetDestination(playerTransform.position);

        transform.localScale = bodyScale;

        if (timeElapsed >= 10.0f || health == 0)
        {
            Flash();
            GameObject.FindWithTag("Captain").GetComponent<Captain>().curAirSupport -= 1;
            Destroy(gameObject);
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("ApplyDamage", 2);
            other.gameObject.SendMessage("Flash");
        }
    }

    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
