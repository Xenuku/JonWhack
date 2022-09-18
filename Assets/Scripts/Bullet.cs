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
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Enemy")
        {
            // Take the enemies health down
            //other.collider.gameObject.GetComponent<MeleeEnemy>().health -= 10;
            other.collider.gameObject.GetComponent<Sniper>().health -= 10;

            // Destroy the bullet
            Destroy(gameObject);
        }
        else
        {
            // if it hits another obstacle, just destroy it
            Destroy(gameObject);
        }

    }
}
