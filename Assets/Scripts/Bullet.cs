using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float speed;
    public float lifeTime;
    private Vector2 newPos;

    private GameObject slot1;

    private Vector3Int contact;

    public enum EnemyTypes 
    {
        Melee,
        Sniper,
        Heavy,
        Player,
        Wall,
        Center,
        Support,
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
            case "Wall":
                other.collider.gameObject.GetComponent<Wall>().health -= damage;
                other.collider.gameObject.SendMessage("Flash");
                Destroy(gameObject);
                break;
            case "Center":
                other.collider.gameObject.GetComponent<Center>().health -= damage;
                other.collider.gameObject.SendMessage("Flash");
                Destroy(gameObject);
                break;
            // case "Support":
            //     // Will take 2 shots to kill
            //     other.collider.gameObject.GetComponent<Support>().health -= damage;
            //     other.collider.gameObject.SendMessage("Flash");
            //     Destroy(gameObject);
            //     break;
            // case "Melee":
            //     // Will take 2 shots to kill
            //     other.collider.gameObject.GetComponent<MeleeEnemy>().health -= damage;
            //     other.collider.gameObject.SendMessage("Flash");
            //     Destroy(gameObject);
            //     break;
            // case "Sniper":
            //     // Will take 1 shot to kill
            //     other.collider.gameObject.GetComponent<Sniper>().health -= damage;
            //     other.collider.gameObject.SendMessage("Flash");
            //     Destroy(gameObject);
            //     break;
            // case "Heavy":
            //     other.collider.gameObject.GetComponent<Heavy>().health -= damage;
            //     other.collider.gameObject.SendMessage("Flash");
            //     Destroy(gameObject);
            //     break;
            case "Player":
                other.collider.gameObject.GetComponent<PlayerController>().health -= damage;
                other.collider.gameObject.SendMessage("Flash");
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
