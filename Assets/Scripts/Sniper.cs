using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Sniper : MonoBehaviour
{
    public State curState;
    public float shootRate = 3.0f;
    protected float elapsedTime;
    public int health = 30;
    // ranges
    public float attackRange = 10.0f;
    public float attackRangeStop = 5.0f;
    public int moving = 0;
    private Transform playerTransform;
    private Transform SpawnManager;
    public SpriteRenderer sprite;

    // This enemy is worth 150 xp
    public int exp_worth = 150;
    public int score_worth;
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

    public GameObject scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        speed = 3f;
        playerTransform = GameObject.Find("Player").transform;
        SpawnManager = GameObject.Find("SpawnManager").transform;
        scoreManager = GameObject.Find("ScoreManager");
        curState = State.follow;
        elapsedTime = 0.0f;
        score_worth = exp_worth * 2;
        dist = Vector2.Distance(transform.position, playerTransform.position);

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

        }

        // If no health, switch to dead state
        if (health <= 0)
        {
            curState = State.dead;
        }
        AimSniper();
    }

    protected void AimSniper()
    {
        // Aim sniper at player
        Vector3 aimDirection = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        sniperRifle.transform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1f;
            localScale.x = -1f;
        }
        else
        {
            localScale.y = +1f;
            localScale.x = +1f;
        }
        sniperRifle.transform.localScale = localScale;
    }
    protected void UpdateFollowState()
    {
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
        SpawnManager.gameObject.SendMessage("reduceEnemy", (int)1);
        scoreManager.GetComponent<ScoreManager>().AddToScore(score_worth);
        Destroy(gameObject);
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
                sniperBullet.GetComponent<Rigidbody2D>().velocity = direction * 20.0f;
            }
            elapsedTime = 0.0f;
        }
    }
    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
