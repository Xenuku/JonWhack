using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Sniper : MonoBehaviour
{
    public enum State
    {
        Follow,
        Attack,
        Dead,
    }
    public State curState;
    protected Transform playerTransform;// Player Transform
    //gun
    public GameObject gun;
    public float gunRotSpeed = 4.0f;
    //bullet
    public GameObject bullet;
    public GameObject bulletSpawnPoint;

    // Bullet shooting rate
    public float shootRate = 3.0f;
    protected float elapsedTime;

    // Whether the NPC is destroyed or not
    protected bool bDead;
    public int health = 100;

    public float attackRange = 20.0f;
    private NavMeshAgent nav;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        curState = State.Follow;
        bDead = false;
        elapsedTime = 0.0f;
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;
        speed=3;

        if (!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");


        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case State.Follow: UpdateChaseState(); break;//needto fix

            //case State.Chase: UpdateChaseState(); break;
            //case State.Attack: UpdateAttackState(); break;
            //case State.Dead: UpdateDeadState(); break;
        }
        elapsedTime += Time.deltaTime;
        if (health <= 0)
        {
            curState = State.Dead;
        }
    }
    protected void UpdateChaseState()
    {
      if (playerTransform != null)
        {

            //float distance = Vector3.Distance(transform.position, playerTransform.transform.position);
            //nav.speed = movingSpeed;
            //nav.destination = playerTransform.position;
            //StartCoroutine(Reset());
           // nav.SetDestination(playerTransform.position);
            float dir = playerTransform.position.x-transform.position.x;
            float ydir=playerTransform.position.y-transform.position.y;

            dir = (dir < 0) ? -1 : 1;
            ydir = (ydir < 0) ? -1 : 1;

            transform.Translate(new Vector2(dir, ydir) * speed * Time.deltaTime);

        }
    }
      
}
