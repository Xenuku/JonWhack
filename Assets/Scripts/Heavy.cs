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
        hired,
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
    public bool enchantLooking = false;
    private bool enchanted = false;
    public Vector3 centerTransform;
    public bool hired = false;
    public Vector2 battlePosition;

    //references
    private Transform playerTransform;
    private GameObject SpawnManager;
    public Animator animator;
    public NavMeshAgent enemyAgent;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    public GameObject scoreManager;
    public SpriteRenderer sprite;
    public GameObject sword;
    public GameObject shield;
    public GameObject Enhencedbullet;
    public GameObject blood;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        scoreManager = GameObject.Find("ScoreManager");
        SpawnManager = GameObject.Find("SpawnManager");
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
            case State.hired: UpdateHiredState(); break;
        }

        if (hired == true)
        {
            curState = State.hired;
        }

        if (health <= 0)
        {
            curState = State.dead;
        }
    }

    protected void UpdateHiredState()
    {
        dist = Vector2.Distance(transform.position, battlePosition);
        enemyAgent.SetDestination(battlePosition);
        enemyAgent.stoppingDistance = 1.0f;

        if (dist >= 1.0f)
        {
            animator.SetBool("IsAttack", false);
        }
        else
        {
            animator.SetBool("IsAttack", true);
            SHOOTBULLET();
        }

        if (hired == false)
        {
            curState = State.follow;
        }
    }

    protected void UpdateFollowState()
    {
        animator.SetBool("IsAttack", false);

        if (enchantLooking == true && enchanted == false)
        {
            dist = Vector2.Distance(transform.position, centerTransform);
            enemyAgent.SetDestination(centerTransform);

            if (dist <= 5.0f)
            {
                healFlash();
                enchanted = true;
                health += 100;
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
        if (dist < attackRange)
        {
            curState = State.attack;
        }
    }

    protected void UpdateAttackState()
    {
        dist = Vector2.Distance(transform.position, playerTransform.position);
        animator.SetBool("IsAttack", true);


        if (enchanted == false)
        {
            ShootBullet();
        }
        else
        {
            SHOOTBULLET();
        }

        if (dist > attackRange)
        {
            curState = State.follow;
        }
    }

    protected void UpdateDeadState()
    {
        playerTransform.gameObject.SendMessage("GiveEXP", (int)exp_worth);
        scoreManager.GetComponent<ScoreManager>().AddToScore(score_worth);
        SpawnManager.GetComponent<SpawnManager>().curEliteNum -= 1;

        GameObject Blood = (GameObject)Instantiate(blood, transform.position, Quaternion.identity);
        blood.transform.parent = null;
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

                Bullet.GetComponent<Rigidbody2D>().velocity = direction * 10.0f;
            }

            timeElapsed = 0.0f;
        }
    }

    private void SHOOTBULLET()
    {
        if (timeElapsed >= shootRate)
        {
            if ((bullet))
            {
                Vector2 direction = (Vector2)((playerTransform.position - transform.position));
                direction.Normalize();

                GameObject Bullet = (GameObject)Instantiate(
                                    Enhencedbullet,
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

    public IEnumerator healFlash()
    {
        sprite.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
