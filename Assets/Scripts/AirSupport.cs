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

        //setup navmesh AI, because this is a 2D game so some variables need to be locked
        enemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyAgent.updateRotation = false;
        enemyAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        //rendersprite always facing player
        Vector3 bodyScale = transform.localScale;
        if (playerTransform.position.x >= transform.position.x)
        {
            sprite.flipY = false;
        }
        else
        {
            sprite.flipY = true;
        }

        //this enemy always chasing player
        enemyAgent.SetDestination(playerTransform.position);

        transform.localScale = bodyScale;

        if (timeElapsed >= 10.0f || health <= 0)
        {
            //flash 3 times upon reaching lifetime limit, also send a message to captain say hes dead
            Flash();
            Flash();
            Flash();
            GameObject.FindWithTag("Captain").GetComponent<Captain>().curAirSupport -= 1;
            Destroy(gameObject, 2.0f);
        }
    }

    //do collision damage to player
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("ApplyDamage", 1);
            other.gameObject.SendMessage("Flash");
        }
    }

    //reset velocity after knockback 
    public IEnumerator resetVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    //flash effects for damage taken
    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
