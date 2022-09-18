using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public class MeleeEnemy : MonoBehaviour
{
    //this is for close attack enemy unit.
    public State curState;
    public GameObject Enemy;
    public float shootRate = 3.0f;
    protected float elapsedTime;
    protected bool bDead;
    public int health = 10;
    // ranges
    public float attackRange = 5.0f;
    public float attackRangeStop = 2.5f;
    public int moving = 0;

    private NavMeshAgent nav;
    private Transform playerTransform;
    // This enemy is worth 100 xp
    public int exp_worth = 100;
    public enum State
    {
        follow,
        attack,
        dead,
    }
    State state;
    public float speed;

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
        // Always want the enemies following and attacking
        UpdateChaseState();
        if (health <= 0)
        {
            curState = State.dead;
        }
        float dis = Vector3.Distance(transform.position, playerTransform.position);
        
        // flip sprite depending which way the enemy is walking
        Vector3 localScale = Vector3.one;
        if(playerTransform.position.x >= transform.position.x) {
            localScale.x = -1f;
        } else {
            localScale.x = +1f;
        }
        transform.localScale = localScale;
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
            float dir = playerTransform.position.x - transform.position.x;
            float ydir = playerTransform.position.y - transform.position.y;

            dir = (dir < 0) ? -1 : 1;
            ydir = (ydir < 0) ? -1 : 1;

            transform.Translate(new Vector2(dir, ydir) * speed * Time.deltaTime);
        }
    }

    protected void UpdateDeadState()
    {
        // Show the dead animation with some physics effects
        if (!bDead)
        {
            bDead = true;
            playerTransform.gameObject.SendMessage("GiveEXP", (int) exp_worth);
            //nav.enabled = false;
            Destroy(gameObject);
        }
    }
    IEnumerator Reset()
    {
        nav.isStopped = true;
        yield return new WaitForSeconds(2);
        nav.isStopped = false;
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
    }
    void OnCollisionStay2D(Collision2D other) {
        if (other.collider.gameObject.tag == "Player"){
            other.gameObject.SendMessage("ApplyDamage", 1);
            speed = 0;
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        if(other.collider.gameObject.tag == "Player") {
            speed = 1;
        }
    }
}
