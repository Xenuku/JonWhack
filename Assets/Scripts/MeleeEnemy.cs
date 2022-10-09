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
    private Vector2 dashPosition;

    // This enemy is worth 100 xp
    public int exp_worth = 100;
    public int score_worth;


    //references
    public Vector3 centerTransform;
    public Animator animator;
    public GameObject sword;
    public GameObject shield;
    public SpriteRenderer sprite;
    public GameObject scoreManager;
    private Transform playerTransform;
    private GameObject SpawnManager;
    public GameObject blood;

    //AI
    public UnityEngine.AI.NavMeshAgent enemyAgent;

    //FSM variable to store all states
    public enum State
    {
        follow,
        attack,
        dead,
    }
    State state;

    void Start()
    {
        //setup references
        score_worth = exp_worth * 2;
        playerTransform = GameObject.Find("Player").transform;
        SpawnManager = GameObject.Find("SpawnManager");
        scoreManager = GameObject.Find("ScoreManager");
        curState = State.follow;
        elapsedTime = 0.0f;

        //setup AI setting, locked rotation because this is a 2D game.
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

        //In every frame if health is lower than 0, switch to dead mode
        if (health <= 0)
        {
            curState = State.dead;
        }

        //Ddjust render to always face player 
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
        //where this enemy perform his dash ability
        //if time elasped longer than 10s, they will start dash with animation setup then a desitination update to player
        if (elapsedTime >= 10.0f)
        {
            animator.SetBool("Dash", true);
            enemyAgent.SetDestination(playerTransform.position);

            //setup speed for dash duration
            enemyAgent.speed = 20.0f;
            enemyAgent.acceleration = 10.0f;


            if (elapsedTime >= 12.5f)
            {
                //turn off dash animation after 2.5s 
                animator.SetBool("Dash", false);
                animator.SetBool("Cooldown", true);

                //enemy rest status setup and enter cooldown
                enemyAgent.speed = 0.0f;
                enemyAgent.SetDestination(playerTransform.position);
                elapsedTime = 0.0f;
            }
        }

        //enemy recover from reset and continues chasing player until cooldown finish
        if (elapsedTime >= 2.0f && elapsedTime < 10.0f)
        {
            animator.SetBool("Cooldown", false);
            enemyAgent.SetDestination(playerTransform.position);
            enemyAgent.speed = 4.0f;
            enemyAgent.acceleration = 8.0f;
        }

        //back to follow state if out of range
        if (dist > 10.0f)
        {
            curState = State.follow;
        }
    }

    protected void UpdateChaseState()
    {
        //check if this enemy selected as enchant target and if he's already enchanted
        if (enchantLooking == true && enchanted == false)
        {
            //store and update distance with selected support center
            dist = Vector2.Distance(transform.position, centerTransform);
            enemyAgent.SetDestination(centerTransform);

            //check if within receiving range
            if (dist <= 5.0f)
            {
                healFlash();
                enchanted = true;
                health += 50;
                sword.SetActive(true);
                shield.SetActive(true);
            }

        }
        else
        {
            //else just act with normal following
            dist = Vector2.Distance(transform.position, playerTransform.position);
            enemyAgent.SetDestination(playerTransform.position);
        }

        // Switch to attack if in range with random start dash cooldown
        if (dist <= 10.0f)
        {
            curState = State.attack;
            elapsedTime = Random.Range(0.0f, 9.0f);
        }
    }

    protected void UpdateDeadState()
    {
        //update EXP, score, current enemy number to system
        playerTransform.gameObject.SendMessage("GiveEXP", (int)exp_worth);
        scoreManager.GetComponent<ScoreManager>().AddToScore(score_worth);
        SpawnManager.GetComponent<SpawnManager>().curEnemyNum -= 1;

        //de-attach blood effects from this enemy so it can finish playing after destory
        GameObject Blood = (GameObject)Instantiate(blood, transform.position, Quaternion.identity);
        blood.transform.parent = null;
        Destroy(gameObject);
    }

    //check if collide with player
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("ApplyDamage", 1);
            other.gameObject.SendMessage("Flash");
        }
    }

    //red flash effects
    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    //reset velocity after knockback 
    public IEnumerator resetVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    
    //green flash for heal
    public IEnumerator healFlash()
    {
        sprite.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
