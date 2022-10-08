using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Support : MonoBehaviour
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
    protected float timeElapsed = 99.0f;
    private bool setup = false;

    //enemy data
    public int health;
    public int score_worth;
    public float shootRate;
    public float attackRange;
    public int exp_worth;
    protected bool Dead;
    private float randomX;
    private float randomY;
    public bool enchantLooking = false;
    private bool enchanted = false;
    private Vector3 destination;
    private Vector3 locations;
    public bool hired = false;
    public Vector2 battlePosition;

    //references
    private Transform playerTransform;
    private GameObject SpawnManager;
    public Vector3 centerTransform;
    private GameObject scoreManager;
    public GameObject wall;
    public GameObject center;
    public GameObject buildingSpawnPoint1;
    public GameObject buildingSpawnPoint2;
    public GameObject buildingSpawnPoint3;
    public GameObject buildingSpawnPoint4;
    public Animator animator;
    public UnityEngine.AI.NavMeshAgent enemyAgent;
    public SpriteRenderer sprite;
    public GameObject bullet;
    public GameObject Enhencedbullet;
    public GameObject bulletSpawnPoint;
    public GameObject sword;
    public GameObject shield;


    // Start is called before the first frame update
    void Start()
    {
        exp_worth = 300;
        score_worth = exp_worth * 2;
        sword.SetActive(false);
        shield.SetActive(false);

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
        // if (!SpawnManager)
        // {
        //     print("respawn doesn't exist.. Please add one with Tag named 'respawn'");
        // }
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

        if (setup == false)
        {
            SetBuildings();
        }
        else
        {
            if (dist >= 1.0f)
            {
                animator.SetBool("IsAttack", false);
            }
            else
            {
                animator.SetBool("IsAttack", true);
                SHOOTBULLET();
            }
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
                enchanted = true;
                health += 50;
                enemyAgent.speed = 7.0f;
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
        
        if (setup == false)
        {
            SetBuildings();
        }
        else
        {
            if (enchanted == false)
            {
                ShootBullet();
            }
            else
            {
                SHOOTBULLET();
            }
        }

        if (dist > attackRange)
        {
            curState = State.follow;
        }
    }

    private void SetBuildings()
    {
        GameObject building1 = (GameObject)Instantiate(center, transform.position, Quaternion.identity);
        locations = transform.position;
        GameObject building2 = (GameObject)Instantiate(wall, buildingSpawnPoint1.transform.position, Quaternion.identity);
        GameObject building3 = (GameObject)Instantiate(wall, buildingSpawnPoint2.transform.position, Quaternion.identity);
        GameObject building4 = (GameObject)Instantiate(wall, buildingSpawnPoint3.transform.position, Quaternion.identity);
        GameObject building5 = (GameObject)Instantiate(wall, buildingSpawnPoint4.transform.position, Quaternion.identity);
        setup = true;
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


    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    protected void UpdateDeadState()
    {
        playerTransform.gameObject.SendMessage("GiveEXP", (int)exp_worth);
        scoreManager.GetComponent<ScoreManager>().AddToScore(score_worth);
        SpawnManager.GetComponent<SpawnManager>().curEliteNum -= 1;

        Destroy(sword.gameObject);
        Destroy(shield.gameObject);
        Destroy(buildingSpawnPoint1.gameObject);
        Destroy(buildingSpawnPoint2.gameObject);
        Destroy(buildingSpawnPoint3.gameObject);
        Destroy(buildingSpawnPoint4.gameObject);
        transform.DetachChildren();
        Destroy(gameObject);
    }

}
