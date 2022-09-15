using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Movement code inspired by: https://www.youtube.com/watch?v=whzomFgjT50 

public class PlayerController : MonoBehaviour
{
    protected float elapsedTime;
    public Rigidbody2D rb;
    public float shootRate = 0.5f;
    public float moveSpeed = 5f;
    private Vector2 movement;
    private const float MAXHEALTH = 100f;
    public float health;
    private Image healthBar;

    public void Start ()
    {
        health = MAXHEALTH;
        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        elapsedTime = shootRate; // First shot can fire (thanks Dimitri)
    }
    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / MAXHEALTH;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        // Back to main menu on 'Escape' key press
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
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
