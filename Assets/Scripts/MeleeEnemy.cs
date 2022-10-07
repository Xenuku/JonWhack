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
    public int health;
    public bool enchantLooking = false;
    private bool enchanted = false;
    private float dist;

    // This enemy is worth 100 xp
    public int exp_worth = 100;
    public int score_worth;


    //references
    public Transform centerTransform;
    public Animator animator;
    public GameObject sword;
    public GameObject shield;
    public SpriteRenderer sprite;
    public GameObject scoreManager;
    private Transform playerTransform;
    private Transform SpawnManager;

    //AI
    public UnityEngine.AI.NavMeshAgent enemyAgent;

    public enum State
    {
        follow,
        attack,
        dead,
    }
    State state;

    void Start()
    {
        score_worth = exp_worth * 2;
        playerTransform = GameObject.Find("Player").transform;
        SpawnManager = GameObject.Find("SpawnManager").transform;
        scoreManager = GameObject.Find("ScoreManager");
        curState = State.follow;
        elapsedTime = 0.0f;

        enemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyAgent.updateRotation = false;
        enemyAgent.updateUpAxis = false;


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
        elapsedTime += Time.deltaTime;

        switch (curState)
        {
            case State.attack: UpdateAttackState(); break;
            case State.follow: UpdateChaseState(); break;
            case State.dead: UpdateDeadState(); break;
        }

        if (health <= 0)
        {
            curState = State.dead;
        }

        Vector3 bodyScale = new Vector3(0.04f, 0.04f, 0);
        if (playerTransform.position.x >= transform.position.x)
        {
            bodyScale.x = +0.04f;
        }
        else
        {
            bodyScale.x = -0.04f;
        }
        transform.localScale = bodyScale;
    }

    protected void UpdateAttackState()
    {
        animator.SetBool("IsAttack", true);

        dist = Vector2.Distance(transform.position, playerTransform.position);
        enemyAgent.SetDestination(playerTransform.position);

        if (dist > 1.0f)
        {
            curState = State.follow;
        }
    }

    protected void UpdateChaseState()
    {
        animator.SetBool("IsAttack", false);

        if (enchantLooking == true && enchanted == false)
        {
            dist = Vector2.Distance(transform.position, centerTransform.position);
            enemyAgent.SetDestination(centerTransform.position);

            if (dist < 5.0f)
            {
                enchanted = true;
                health += 50;
                enemyAgent.speed = 6.0f;
                sword.SetActive(true);
                shield.SetActive(true);
            }

        }
        else
        {
            dist = Vector2.Distance(transform.position, playerTransform.position);
            enemyAgent.SetDestination(playerTransform.position);
        }

        // Switch to attack if in range
        if (dist <= 1.0f)
        {
            curState = State.attack;
        }
    }

    protected void UpdateDeadState()
    {
        playerTransform.gameObject.SendMessage("GiveEXP", (int)exp_worth);
        scoreManager.GetComponent<ScoreManager>().AddToScore(score_worth);
        SpawnManager.gameObject.SendMessage("reduceEnemy", (int)1);
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("ApplyDamage", 1);
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
