using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Sniper : MonoBehaviour
{
    public State curState;
    public GameObject Enemy;
    public float shootRate = 3.0f;
    protected float elapsedTime;
    protected bool bDead;
    public int health = 10;
    // ranges
    public float attackRange = 10.0f;
    public float attackRangeStop = 5.0f;
    public int moving = 0;

    private NavMeshAgent nav;
    private Transform playerTransform;
    // This enemy is worth 150 xp
    public int exp_worth = 150;
    public enum State
    {
        follow,
        attack,
        dead,
    }
    State state;
    public float speed;
    private float dist;


    public GameObject bullet;
    public GameObject bulletSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1;
        playerTransform = GameObject.Find("Player").transform;

        curState = State.follow;
        nav = GetComponent<NavMeshAgent>();
        bDead = false;
        elapsedTime = 0.0f;

        //GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        //playerTransform = objPlayer.transform;
        //Debug.Log(playerTransform);

        if (!playerTransform)
        {
            print("Player doesn't exist.. Please add one with Tag named 'Player'");
        }

        dist = Vector2.Distance(transform.position, playerTransform.position);
        //Debug.Log(dist);

    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector2.Distance(transform.position, playerTransform.position);

        switch (curState)
        {
            case State.follow: UpdateFollowState(); break;
            case State.dead: UpdateDeadState(); break;
            case State.attack: UpdateAttackState(); break;

        }

        elapsedTime += Time.deltaTime;

        // If no health, switch to dead state
        if (health <= 0)
        {
            curState = State.dead;
        }

        // flip sprite depending which way the enemy is walking
        Vector3 localScale = Vector3.one;
        if (playerTransform.position.x >= transform.position.x)
        {
            localScale.x = -1f;
        }
        else
        {
            localScale.x = +1f;
        }
        transform.localScale = localScale;
    }
    protected void UpdateFollowState()
    {
        Debug.Log("Follow state, dist: " + dist);
        if (playerTransform != null)
        {
            float dir = playerTransform.position.x - transform.position.x;
            float ydir = playerTransform.position.y - transform.position.y;

            dir = (dir < 0) ? -1 : 1;
            ydir = (ydir < 0) ? -1 : 1;

            transform.Translate(new Vector2(dir, ydir) * speed * Time.deltaTime);
        }
        // Switch to attack if in range
        if (dist < attackRange)
        {
            curState = State.attack;
        }
        Debug.DrawLine(playerTransform.position, transform.position);
    }
    protected void UpdateAttackState()
    {
        Debug.Log("Attack state, dist: " + dist);
        // Switch to follow if not in range to attack 
        if (dist > attackRange)
        {
            curState = State.follow;
        }
        ShootBullet();
    }

    protected void UpdateDeadState()
    {
        if (!bDead)
        {
            bDead = true;
            playerTransform.gameObject.SendMessage("GiveEXP", (int)exp_worth);
            //nav.enabled = false;
            Destroy(gameObject);
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
    }
   
    private void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            if ((bullet))
            {
                Vector2 direction = (Vector2)((playerTransform.position - transform.position));
                direction.Normalize();
                GameObject sniperBullet = (GameObject)Instantiate(
                                    bullet,
                                    bulletSpawnPoint.transform.position + (Vector3)(direction * 0.5f),
                                    Quaternion.identity);
                sniperBullet.GetComponent<Rigidbody2D>().velocity = direction * 5.0f;
            }
            elapsedTime = 0.0f;
        }
    }
}
