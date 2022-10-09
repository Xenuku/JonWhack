using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Captain : MonoBehaviour
{
    //AI related variables
    public enum State
    {
        follow,
        attack,
        dead,
    }
    public State curState;
    private float dist;
    public int score_worth;

    //system
    protected float timeElapsed = 20.0f;

    //enemy data
    public int health;
    public float attackRange;
    public int exp_worth;
    protected bool Dead;

    //references
    private Transform playerTransform;
    public Animator animator;
    public UnityEngine.AI.NavMeshAgent enemyAgent;
    private GameObject SpawnManager;
    public GameObject scoreManager;
    public GameObject AttackSlot1;
    public GameObject AttackSlot2;
    public GameObject AttackSlot3;

    private GameObject hire1;
    private GameObject hire2;
    private GameObject hire3;
    public SpriteRenderer sprite;
    public GameObject blood;


    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager");
        playerTransform = GameObject.Find("Player").transform;
        SpawnManager = GameObject.Find("SpawnManager");
        curState = State.follow;

        //setup navmesh AI, because this is a 2D game so some variables need to be locked
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
        //setup distance for future use
        dist = Vector2.Distance(transform.position, playerTransform.position);
        timeElapsed += Time.deltaTime;

        //spriterender always facing player
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

        switch (curState)
        {
            case State.follow: UpdateFollowState(); break;
            case State.dead: UpdateDeadState(); break;
            case State.attack: UpdateAttackState(); break;
        }

        //switch to death state if health lower than 0 in any frame
        if (health <= 0)
        {
            curState = State.dead;
        }
    }

    protected void UpdateFollowState()
    {
        //setup captain animation as walking
        animator.SetBool("IsAttack", false);

        //set navmesh destination as play
        enemyAgent.SetDestination(playerTransform.position);

        //hire enemies into battle squad
        Hire();

        // Switch to attack if in range
        if (dist < attackRange)
        {
            curState = State.attack;
        }
    }

    protected void UpdateAttackState()
    {
        //set captain attack animation on
        animator.SetBool("IsAttack", true);

        //continue hire enemies if current position is empty
        Hire();

        //if captain's ability finished cool down
        if(timeElapsed >= 30.0f)
        {
            //call air supports to attack player 
            airSupport();

            //give squad member healings and trigger their healing flash effects
            hire1.GetComponent<Sniper>().health += 50;
            hire1.SendMessage("healFlash");
            hire2.GetComponent<Heavy>().health += 80;
            hire2.SendMessage("healFlash");
            hire3.GetComponent<Support>().health += 60;
            hire3.SendMessage("healFlash");

            //reset skills cooldown
            timeElapsed = 0.0f;
        }

        //switch back to follow state if out of attackRange
        if (dist >= attackRange)
        {
            curState = State.follow;
        }
    }

    protected void UpdateDeadState()
    {
        //update EXP, score, current enemy number to system
        playerTransform.gameObject.SendMessage("GiveEXP", (int)exp_worth);
        scoreManager.GetComponent<ScoreManager>().AddToScore(score_worth);
        SpawnManager.GetComponent<SpawnManager>().curCaptainNum -= 1;

        //let squad member know he's dead and they are no longer members
        hire1.GetComponent<Sniper>().hired = false;
        hire2.GetComponent<Heavy>().hired = false;
        hire3.GetComponent<Support>().hired = false;

        //generate death effect
        GameObject Blood = (GameObject)Instantiate(blood, transform.position, Quaternion.identity);
        blood.transform.parent = null;
        Destroy(gameObject);
    }
    
    //variables for air supports

    private Vector2 spawnPosition;
    private float playerDistance;
    public int curAirSupport;
    public int maxAirSupport;
    public GameObject AirSupport;

    protected void airSupport()
    {
        //random generate positions, similar to spawn system 
        spawnPosition.x = Random.Range(Camera.main.transform.position.x - 40.0f, Camera.main.transform.position.x + 40.0f);
        spawnPosition.y = Random.Range(Camera.main.transform.position.y - 40.0f, Camera.main.transform.position.y + 40.0f);

        playerDistance = Vector2.Distance(spawnPosition, playerTransform.position);

        //re-generate if too close
        while (playerDistance <= 20.0f)
        {
            spawnPosition.x = Random.Range(Camera.main.transform.position.x - 40.0f, Camera.main.transform.position.x + 40.0f);
            spawnPosition.y = Random.Range(Camera.main.transform.position.y - 40.0f, Camera.main.transform.position.y + 40.0f);
            playerDistance = Vector2.Distance(spawnPosition, playerTransform.position);
        }

        //air support will only be generated if none air supports exist on the battle field
        if (curAirSupport == 0)
        {
            GameObject Airplane1 = (GameObject)Instantiate(AirSupport, spawnPosition, Quaternion.identity);
            GameObject Airplane2 = (GameObject)Instantiate(AirSupport, spawnPosition, Quaternion.identity);
            GameObject Airplane3 = (GameObject)Instantiate(AirSupport, spawnPosition, Quaternion.identity);

            curAirSupport += 3;
        }
    }

    protected void Hire()
    {
        //hire enemies if they are exist, messages sent to let they know they are hired
        if (GameObject.FindWithTag("Sniper") != null)
        {
            hire1 = GameObject.FindWithTag("Sniper");
            hire1.GetComponent<Sniper>().hired = true;         
            hire1.GetComponent<Sniper>().battlePosition = AttackSlot1.transform.position;
        }

        if (GameObject.FindWithTag("Heavy") != null)
        {
            hire2 = GameObject.FindWithTag("Heavy");
            hire2.GetComponent<Heavy>().hired = true;
            hire2.GetComponent<Heavy>().battlePosition = AttackSlot2.transform.position;
        }

        if (GameObject.FindWithTag("Support") != null)
        {
            hire3 = GameObject.FindWithTag("Support");
            hire3.GetComponent<Support>().hired = true;
            hire3.GetComponent<Support>().battlePosition = AttackSlot3.transform.position;
        }
    }

    //reset velocity after knockback
    public IEnumerator resetVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    //damage flash
    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
