using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Heavy : MonoBehaviour
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
    public float shootRate;
    public float attackRange;
    public float attackRangeStop;
    public int exp_worth = 400;
    public int score_worth;
    protected bool Dead;

    //references
    private Transform playerTransform;
    private Transform SpawnManager;
    public Animator animator;
    public NavMeshAgent enemyAgent;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    public GameObject scoreManager;
    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        scoreManager = GameObject.Find("ScoreManager");
        //SpawnManager = GameObject.Find("SpawnManager").transform;
        curState = State.follow;
        Dead = false;
        score_worth = exp_worth * 2;
        //Navmesh
        enemyAgent = GetComponent<NavMeshAgent>();
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
            animator.SetBool("IsAttack", false);
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

        ShootBullet();

        if (dist > attackRange)
        {
            curState = State.follow;
        }
    }

    protected void UpdateDeadState()
    {
        playerTransform.gameObject.SendMessage("GiveEXP", (int)exp_worth);
        scoreManager.GetComponent<ScoreManager>().AddToScore(score_worth);
        //SpawnManager.gameObject.SendMessage("reduceEnemy", (int)3);
        Destroy(gameObject);
    }

    private void ShootBullet()
    {
        if (timeElapsed >= shootRate)
        {
            if ((bullet))
            {
                Vector2 direction = (Vector2)((playerTransform.position - transform.position));
                direction.Normalize();

                GameObject Bullet = (GameObject)Instantiate(
                                    bullet,
                                    bulletSpawnPoint.transform.position + (Vector3)(direction * 0.5f),
                                    Quaternion.identity);

                Bullet.GetComponent<Rigidbody2D>().velocity = direction * 5.0f;
            }

            timeElapsed = 0.0f;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("ApplyDamage", 1);
        }
    }

    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
