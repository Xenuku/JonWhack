using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float speed = 300.0f;

    public float lifeTime = 2.0f;
    private Vector2 newPos;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        newPos=transform.position +transform.forward*speed*Time.deltaTime;
        RaycastHit hit;
		if (Physics.Linecast(transform.position, newPos, out hit))
        {
            if (hit.collider)
            {
                transform.position=hit.point;
                Destroy(gameObject);
                GameObject obj=hit.collider.gameObject;
                if(obj.tag=="enemy1"){
                    MeleeEnemy enemy=(MeleeEnemy) obj.GetComponent(typeof(MeleeEnemy));
                    enemy.ApplyDamage(damage);
                    Debug.Log("Hit");
                    
                    }
            }
        }else{
        transform.position=newPos;
        }
    }
}
