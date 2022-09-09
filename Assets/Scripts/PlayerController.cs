using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 speed = new Vector2(50, 50);
    public GameObject pBullet;
    public float bulletSpeed = 30.0f;
    protected float elapsedTime;
    public float shootRate = 0.5f;
    

    public void Start ()
    {
        elapsedTime = shootRate; // First shot can fire (thanks Dimitri)
    }

    // Update is called once per frame
    void Update()
    {

        // Basic movement
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        
        Vector3 move = new Vector3(speed.x * inputX, speed.y * inputY, 0);
        
        move *= Time.deltaTime;
        
        transform.Translate(move);
        
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
}
