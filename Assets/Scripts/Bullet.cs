using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float speed;
    public float lifeTime;
    private Vector2 newPos;

    public enum EnemyTypes 
    {
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
