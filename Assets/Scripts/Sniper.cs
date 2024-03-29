using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Sniper : MonoBehaviour
{
    public State curState;
    public float shootRate;
    protected float elapsedTime;
    public int health;
    // ranges
    public float attackRange;
    public bool enchantLooking = false;
    private bool enchanted = false;
    public bool hired = false;

    // This enemy is worth 150 xp
    public int exp_worth = 150;
    public int score_worth;
    public enum State
    {
        follow,
        attack,
        dead,
        hired,
    }
    State state;
    private float dist;

    //references
    private Transform playerTransform;
    private GameObject SpawnManager;
    public Vector3 centerTransform;
    public Vector2 battlePosition;
    public SpriteRenderer sprite;
    public Animator animator;
    public GameObject enhencedBullet;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;  
    private GameObject scoreManager;
    public GameObject sword;
    public GameObject shield;
    public GameObject blood;

    //AI
    public UnityEngine.AI.NavMeshAgent enemyAgent;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        SpawnManager = GameObject.Find("SpawnManager");
        scoreManager = GameObject.Find("ScoreManager");
        curState = State.follow;
        elapsedTime = 0.0f;
        score_worth = exp_worth * 2;

        //setup AI setting, locked rotation because this is a 2D game
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

         // flip sprite depending which way the enemy is walking
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

        //if enemy is hired he will go to hired state
        if (hired == true && health > 0)
        {
            curState = State.hired;
        }


        // If no health, switch to dead state
        if (health <= 0)
        {
            curState = State.dead;
        }
    }


    protected void UpdateHiredState()
    {
        dist = Vector2.Distance(transform.position, battlePosition);

        //enemy will only stay in his battle position after get hired

        enemyAgent.SetDestination(battlePosition);
        enemyAgent.stoppingDistance = 1.0f;

        if (dist >= 1.0f)
        {
            //play idle animation when they are within the battle position
            animator.SetBool("IsAttack", false);
        }
        else
        {
            //enemy will shoot bullets no matter har far players at
            animator.SetBool("IsAttack", true);
            ShootBullet();
        }

        //back to follow if captain dead
        if (hired == false)
        {
            curState = State.follow;
        }
    }

    protected void UpdateFollowState()
    {
        animator.SetBool("IsAttack", false);

        //looking for suppport center if selected as target

        if (enchantLooking == true && enchanted == false)
        {
            dist = Vector2.Distance(transform.position, centerTransform);
            enemyAgent.SetDestination(centerTransform);

            if (dist <= 5.0f)
            {
                healFlash();
                enchanted = true;
                health += 50;
                enemyAgent.speed = 5.0f;

                //cute icons on indicate they are enchanted
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

        //shoots enchanted bullets after enchant
        ShootBullet();

        if (dist > attackRange)
        {
            curState = State.follow;
        }
    }

    protected void UpdateDeadState()
    {
        playerTransform.gameObject.SendMessage("GiveEXP", (int)exp_worth);
        SpawnManager.GetComponent<SpawnManager>().curEnemyNum -= 1;
        scoreManager.GetComponent<ScoreManager>().AddToScore(score_worth);

        //de-attach blood effects from this enemy so it can finish playing after destory
        GameObject Blood = (GameObject)Instantiate(blood, transform.position, Quaternion.identity);
        blood.transform.parent = null;
        Destroy(gameObject);
    }

    private void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            if ((bullet))
            {
                if (enchanted == false)
                {
                    Vector2 direction = (Vector2)((playerTransform.position - transform.position));
                    direction.Normalize();

                    GameObject Bullet = (GameObject)Instantiate(
                                        bullet,
                                        bulletSpawnPoint.transform.position + (Vector3)(direction * 0.5f),
                                        Quaternion.identity);

                    Bullet.GetComponent<Rigidbody2D>().velocity = direction * 10.0f;
                }
                else if (enchanted == true)
                {
                    Vector2 direction = (Vector2)((playerTransform.position - transform.position));
                    direction.Normalize();

                    GameObject Bullet = (GameObject)Instantiate(
                                        enhencedBullet,
                                        bulletSpawnPoint.transform.position + (Vector3)(direction * 0.5f),
                                        Quaternion.identity);

                    Bullet.GetComponent<Rigidbody2D>().velocity = direction * 5.0f;
                }

            }

            elapsedTime = 0.0f;
        }
    }


    //flash green for heals
    public IEnumerator healFlash()
    {
        sprite.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    //reset velocity after knockback effects
    public IEnumerator resetVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    //flash red for damages
    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
