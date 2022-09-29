using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float speed = 10.0f;
    public float lifeTime = 2.0f;
    private Vector2 newPos;

    private GameObject slot1;

    public enum EnemyTypes 
    {
        Melee,
        Sniper,
        Heavy,
        Player,
    }
    private string enemyType;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        newPos = transform.position + transform.forward * speed * Time.deltaTime;
        transform.position = newPos;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        enemyType = other.collider.gameObject.tag;
        switch(enemyType) {
            case "Melee":
                // Will take 2 shots to kill
                other.collider.gameObject.GetComponent<MeleeEnemy>().health -= 10;
                other.collider.gameObject.SendMessage("Flash");
                Destroy(gameObject);
                break;
            case "Sniper":
                // Will take 1 shot to kill
                other.collider.gameObject.GetComponent<Sniper>().health -= 10;
                other.collider.gameObject.SendMessage("Flash");
                Destroy(gameObject);
                break;
            case "Heavy":
                other.collider.gameObject.GetComponent<Heavy>().health -= 10;
                other.collider.gameObject.SendMessage("Flash");
                Destroy(gameObject);
                break;
            case "Player":
                other.collider.gameObject.GetComponent<PlayerController>().health -= 5;
                other.collider.gameObject.SendMessage("Flash");
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
