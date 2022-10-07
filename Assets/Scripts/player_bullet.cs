using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class player_bullet : MonoBehaviour
{
    public int damage;
    public float speed;
    public float lifeTime;
    private Vector2 newPos;
    public SpriteRenderer BulletSprite;
    Vector3 mousePosition;

    private Vector3Int contact;
    private Transform BulletSpawnPoint;

    public enum EnemyTypes 
    {
        Melee,
        Sniper,
        Heavy,
        Wall,
        Center,
        Support,
    }
    private string enemyType;

    void Start()
    {
        Destroy(gameObject, lifeTime);

        //adjust Bullet animations
        BulletSpawnPoint = transform.Find("GunEndPoint");
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        BulletAimEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        newPos = transform.position + transform.forward * speed * Time.deltaTime;
        transform.position = newPos;
    }
    protected void BulletAimEnemies()
    {
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 localScale = new Vector3(0.05f, 0.05f, 0);

        transform.localScale = localScale;
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
            case "Support":
                // Will take 2 shots to kill
                other.collider.gameObject.GetComponent<Support>().health -= damage;
                other.collider.gameObject.SendMessage("Flash");
                Destroy(gameObject);
                break;
            case "Melee":
                // Will take 2 shots to kill
                other.collider.gameObject.GetComponent<MeleeEnemy>().health -= damage;
                other.collider.gameObject.SendMessage("Flash");
                Destroy(gameObject);
                break;
            case "Sniper":
                // Will take 1 shot to kill
                other.collider.gameObject.GetComponent<Sniper>().health -= damage;
                other.collider.gameObject.SendMessage("Flash");
                Destroy(gameObject);
                break;
            case "Heavy":
                other.collider.gameObject.GetComponent<Heavy>().health -= damage;
                other.collider.gameObject.SendMessage("Flash");
                Destroy(gameObject);
                break;
            
            default:
                Destroy(gameObject);
                break;
        }
    }
}
