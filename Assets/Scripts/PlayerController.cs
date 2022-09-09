using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// Movement code inspired by: https://www.youtube.com/watch?v=whzomFgjT50 

public class PlayerController : MonoBehaviour
{
    protected float elapsedTime;
    public Rigidbody2D rb;
    public GameObject pBullet;
    public float shootRate = 0.5f;
    public float bulletSpeed = 30.0f;
    public float moveSpeed = 5f;
    private Vector2 movement;
    

    public void Start ()
    {
        elapsedTime = shootRate; // First shot can fire (thanks Dimitri)
    }
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        // Back to main menu on 'Escape' key press
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        // Shooting
        if(Input.GetButton("Fire1")) 
        {
            if (elapsedTime >= shootRate) {
                elapsedTime = 0.0f;
                Vector3 mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (Vector2)((mouseP - transform.position));
                direction.Normalize ();
                GameObject baseBullet = (GameObject)Instantiate (
                                    pBullet,
                                    transform.position + (Vector3)(direction * 0.5f),
                                    Quaternion.identity);
                baseBullet.GetComponent<Rigidbody2D>().velocity = direction * 30.0f;
            }
        }
        elapsedTime += Time.deltaTime;
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + 
                        movement * 
                        moveSpeed * 
                        Time.fixedDeltaTime
                    );
    }
}
