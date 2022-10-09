using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Movement code inspired by: https://www.youtube.com/watch?v=whzomFgjT50 

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    private Vector2 movement;
    public float maxHealth = 100f;
    public float health;
    public int level = 1;
    public int damage;
    public float bulletSpeed;
    public float bonusScore;
    public float fireRate;
    public float experience = 0.0f;
    private int levelExpRequired = 1000;
    //animation
    public Animator animator;

    //references
    private Image healthBar;
    private Image expBar;
    public TMP_Text levelText;
    public SpriteRenderer sprite;
    private GameObject controller;
    Vector3 mousePos;
    public GameObject upgradeManager;
    private bool upgradeChosen;
    
    public void Start ()
    {
        // Set the health to the max health and set up the UI
        health = maxHealth;
        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        expBar = GameObject.Find("ExpBar").GetComponent<Image>();
        controller = GameObject.Find("Controller");
    }
    // Update is called once per frame
    void Update()
    {
        // Get the current value of the weapon being chosen, so the player can't shoot
        // if it is not chosen
        upgradeChosen = upgradeManager.GetComponent<UpgradeManager>().upgradeChosen;
        // Set the gui values according to the current values
        float curExp = (float)experience;
        expBar.fillAmount = curExp / levelExpRequired;
        healthBar.fillAmount = health / maxHealth;
        // Handle the movement of the player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        // Animation for the players states (walking or standing still)
        if (movement.x != 0.0f || movement.y != 0.0f)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
        // If the game is paused or if the starting weapon has not been chosen
        // Then do not allow the players sprite to change
        if (!PauseMenu.gameIsPaused)
        {
            if (upgradeChosen) {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (mousePos.x >= transform.position.x)
                {
                    sprite.flipX = false;
                }
                else
                {
                    sprite.flipX = true;
                }
            }
        }
        // Self explanatory, but once the players health is 0 or less, kill them
        if (health <= 0) {
            KillThePlayer();
        }
    }
    // move our player around the world
    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + 
                        movement * 
                        moveSpeed * 
                        Time.fixedDeltaTime
                    );
        // If we have enough experience, then perform a level up
        if (experience >= levelExpRequired) {
            LevelUp();
        }
    }
    // Once the player is killed, use the GameOver() function in the controller
    // with the KIA value so the scoreboard can reflect the user was killed
    void KillThePlayer() {
        controller.GetComponent<Controller>().GameOver("KIA");
    }

    // Perform a level up, increase the next levels required xp and update GUI
    void LevelUp() {
        float startExp = 1000;
        float difficulty = 1.4f;
        level += 1;
        float curLevel = (float)level;
        experience = 0;
        // Stat increases
        health = maxHealth; // Heal the player on level up
        damage += 2; // increase the players damage slightly
        moveSpeed += 0.2f; // increase the players movement speed slightly
        // Gui Changes, set the level text in the EXP bar to the new level
        levelText.text = "Lv. " + level;
        // New XP required
        levelExpRequired = Mathf.FloorToInt(startExp * Mathf.Pow(curLevel, difficulty));
    }
    // When the player is hit, take health away and flash the players sprite
    void ApplyDamage(int damage) {
        health -= damage;
        Flash();
    }

    // On Enemy death, they will send this to us to trigger XP for Jon
    void GiveEXP(int exp) {
        experience += exp;
    }
    // Flash red very quickly when hit
    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
