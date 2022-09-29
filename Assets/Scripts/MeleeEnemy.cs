using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public class MeleeEnemy : MonoBehaviour
{
    //this is for close attack enemy unit.
    public State curState;
    public GameObject Enemy;
    protected float elapsedTime;
    public int health = 10;
    // ranges
    private Transform playerTransform;
    private Transform SpawnManager;
    // This enemy is worth 100 xp
    public int exp_worth = 100;
    public SpriteRenderer sprite;
    public enum State
    {
        follow,
        attack,
        dead,
    }
    State state;
    public float speed;

    void Start()
    {
        speed = 1;
        playerTransform = GameObject.Find("Player").transform;
        SpawnManager = GameObject.Find("SpawnManager").transform;
        curState = State.follow;
        elapsedTime = 0.0f;
        if (!playerTransform)
        {
            print("Player doesn't exist.. Please add one with Tag named 'Player'");
        }
        if (!SpawnManager)
        {
            print("respawn doesn't exist.. Please add one with Tag named 'respawn'");
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case State.follow: UpdateChaseState(); break;
            case State.dead: UpdateDeadState(); break;
        }
        // Always chasing player
        UpdateChaseState();
        elapsedTime += Time.deltaTime;
        // Always want the enemies following and attacking
        if (health <= 0)
        {
            curState = State.dead;
        }
        // flip sprite depending which way the enemy is walking
        Vector3 localScale = new Vector3(0.25f, 0.28f, 1);
        if (playerTransform.position.x >= transform.position.x)
        {
            localScale.x = -0.25f;
        }
        else
        {
            localScale.x = +0.25f;
        }
        transform.localScale = localScale;
    }

    protected void UpdateChaseState()
    {
        if (playerTransform != null)
        {
            float dir = playerTransform.position.x - transform.position.x;
            float ydir = playerTransform.position.y - transform.position.y;
            dir = (dir < 0) ? -1 : 1;
            ydir = (ydir < 0) ? -1 : 1;
            transform.Translate(new Vector2(dir, ydir) * speed * Time.deltaTime);
        }
    }

    protected void UpdateDeadState()
    {
        playerTransform.gameObject.SendMessage("GiveEXP", (int)exp_worth);
        SpawnManager.gameObject.SendMessage("reduceEnemy", (int)1);
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("ApplyDamage", 1);
            speed = 0;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Player")
        {
            speed = 1;
        }
    }

    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
