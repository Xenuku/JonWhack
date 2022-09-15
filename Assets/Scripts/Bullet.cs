using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float speed = 10.0f;

    public float lifeTime = 2.0f;
    private Vector2 newPos;
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
    private void OnCollisionEnter2D(Collision2D other) {
        print("I hit something " + other.gameObject);
        if (other.collider.gameObject.tag == "Enemy") {
            print("Hit enemy");
            Destroy(gameObject);
            other.gameObject.SendMessage("ApplyDamage", damage);
        } else {
            // if it hits another obstacle, just destroy it
            Destroy(gameObject);
        }
        
    }
}
