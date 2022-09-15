using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public class enemy1 : MonoBehaviour
{
    // Start is called before the first frame update


  
    //this is for close attack enemy unit.
    public State curState;


    public GameObject Enemy;



    public float shootRate = 3.0f;
    protected float elapsedTime;


    protected bool bDead;
    public int health = 100;

    // ranges
    public float attackRange = 5.0f;
    public float attackRangeStop = 2.5f;

    public int moving = 0;
    //___________________________________________________

    private NavMeshAgent nav;
    public Transform playerTransform;


    public enum State
    {
        follow,
        dead,
    }
    State state;
    public static float speed;


    void Start()
    {
        speed=3;

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



    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case State.follow: UpdateChaseState(); break;
            case State.dead: UpdateDeadState(); break;
        }

        elapsedTime += Time.deltaTime;

        if (health <= 0)
        {
            curState = State.dead;
        }
        float dis = Vector3.Distance(transform.position,playerTransform.position);
        if (dis<=10){
            UpdateChaseState();

        }

    }

    protected void UpdateChaseState()
    {
       // nav.speed = 0;
        //playerTransform = GameObject.Find("Player").transform;
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

            transform.Translate(new Vector2(dir, ydir) * enemy1.speed * Time.deltaTime);

        }


    }

    protected void UpdateDeadState()
    {
        // Show the dead animation with some physics effects
        if (!bDead)
        {
            bDead = true;
            nav.enabled = false;
        }
    }
    IEnumerator Reset()
    {
        nav.isStopped = true;
        yield return new WaitForSeconds(2);
        nav.isStopped = false;
    }

    public void ApplyDamage(int damage){
        health-=damage;
        Debug.Log(health);
        
    }
}
