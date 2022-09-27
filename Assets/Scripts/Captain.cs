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

    //system
    protected float timeElapsed = 0.0f;

    //enemy data
    public int health = 50;
    public float shootRate = 3.0f;
    public float attackRange = 2.0f;
    public float attackRangeStop = 5.0f;
    public int exp_worth = 350;
    protected bool Dead;

    //references
    private Transform playerTransform;
    private Transform SpawnManager;
    public Animator animator;
    public UnityEngine.AI.NavMeshAgent enemyAgent;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        //SpawnManager = GameObject.Find("SpawnManager").transform;
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
        if (playerTransform != null)
        {
            enemyAgent.SetDestination(playerTransform.position);
        }
        // Switch to attack if in range
        if (dist < attackRange)
        {
            curState = State.attack;
        }
    }

    protected void UpdateAttackState()
    {
        animator.SetBool("IsAttack", true);

    }

    protected void UpdateDeadState()
    {
    }

}
