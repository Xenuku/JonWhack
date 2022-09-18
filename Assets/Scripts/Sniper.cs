using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Sniper : MonoBehaviour
{
    public State curState;
    public float shootRate = 3.0f;
    protected float elapsedTime;
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

    // Weapon related
    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    public GameObject sniperRifle;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1;
        playerTransform = GameObject.Find("Player").transform;

        curState = State.follow;
        nav = GetComponent<NavMeshAgent>();
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
        Vector3 localScale = new Vector3(0.04f, 0.04f, 1);
        if (playerTransform.position.x >= transform.position.x)
        {
            localScale.x = +0.04f;
        }
        else
        {
            localScale.x = -0.04f;
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
    }
    protected void UpdateAttackState()
    {
        // Temp, need to change this with a red sprite eventually and draw properly
        Debug.DrawLine(playerTransform.position, bulletSpawnPoint.transform.position, Color.red);

        // Aim sniper at player
        Vector3 aimDirection = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        sniperRifle.transform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            //playerSprite.flipX = true;
            localScale.y = -1f;
        }
        else
        {
            //playerSprite.flipX = false;
            localScale.y = +1f;
        }
        sniperRifle.transform.localScale = localScale;
        

        // Switch to follow if not in range to attack 
        if (dist > attackRange)
        {
            curState = State.follow;
        }
        ShootBullet();
    }

    protected void UpdateDeadState()
    {
        playerTransform.gameObject.SendMessage("GiveEXP", (int)exp_worth);
        //nav.enabled = false;
        Destroy(gameObject);
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
