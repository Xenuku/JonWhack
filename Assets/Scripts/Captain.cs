using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Captain : MonoBehaviour
{
    //AI related
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

        //Navmesh
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
        dist = Vector2.Distance(transform.position, playerTransform.position);
        timeElapsed += Time.deltaTime;

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

        if (health <= 0)
        {
            curState = State.dead;
        }
    }

    protected void UpdateFollowState()
    {
        animator.SetBool("IsAttack", false);

        enemyAgent.SetDestination(playerTransform.position);

        Hire();

        // Switch to attack if in range
        if (dist < attackRange)
        {
            curState = State.attack;
        }
    }

    protected void UpdateAttackState()
    {
        animator.SetBool("IsAttack", true);

        Hire();

        if(timeElapsed >= 30.0f)
        {
            airSupport();

            hire1.GetComponent<Sniper>().health += 50;
            hire1.SendMessage("healFlash");
            hire2.GetComponent<Heavy>().health += 80;
            hire2.SendMessage("healFlash");
            hire3.GetComponent<Support>().health += 60;
            hire3.SendMessage("healFlash");

            timeElapsed = 0.0f;
        }

        if (dist >= attackRange)
        {
            curState = State.follow;
        }
    }

    protected void UpdateDeadState()
    {
        playerTransform.gameObject.SendMessage("GiveEXP", (int)exp_worth);
        scoreManager.GetComponent<ScoreManager>().AddToScore(score_worth);
        SpawnManager.GetComponent<SpawnManager>().curCaptainNum -= 1;

        hire1.GetComponent<Sniper>().hired = false;
        hire2.GetComponent<Heavy>().hired = false;
        hire3.GetComponent<Support>().hired = false;

        GameObject Blood = (GameObject)Instantiate(blood, transform.position, Quaternion.identity);
        blood.transform.parent = null;
        Destroy(gameObject);
    }

    private Vector2 spawnPosition;
    private float playerDistance;
    public int curAirSupport;
    public int maxAirSupport;
    public GameObject AirSupport;

    protected void airSupport()
    {
        spawnPosition.x = Random.Range(Camera.main.transform.position.x - 40.0f, Camera.main.transform.position.x + 40.0f);
        spawnPosition.y = Random.Range(Camera.main.transform.position.y - 40.0f, Camera.main.transform.position.y + 40.0f);

        playerDistance = Vector2.Distance(spawnPosition, playerTransform.position);

        while (playerDistance <= 20.0f)
        {
            spawnPosition.x = Random.Range(Camera.main.transform.position.x - 40.0f, Camera.main.transform.position.x + 40.0f);
            spawnPosition.y = Random.Range(Camera.main.transform.position.y - 40.0f, Camera.main.transform.position.y + 40.0f);
            playerDistance = Vector2.Distance(spawnPosition, playerTransform.position);
        }

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

    public IEnumerator resetVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
