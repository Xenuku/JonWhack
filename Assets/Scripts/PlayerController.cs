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

    public void Start ()
    {
        health = maxHealth;
        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        expBar = GameObject.Find("ExpBar").GetComponent<Image>();
        controller = GameObject.Find("Controller");
    }
    // Update is called once per frame
    void Update()
    {
        float curExp = (float)experience;
        expBar.fillAmount = curExp / levelExpRequired;
        healthBar.fillAmount = health / maxHealth;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0.0f || movement.y != 0.0f)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (!PauseMenu.gameIsPaused || !UpgradeManager.upgradeScreenOpen)
        {
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

        // For debugging, press space to take some damage and give yourself XP
        // So you can test the HP and EXP bars.
        if (Input.GetKeyUp(KeyCode.Space)) {
            GiveEXP(100);
            ApplyDamage(10);
        }

        if (health <= 0) {
            KillThePlayer();
        }
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + 
                        movement * 
                        moveSpeed * 
                        Time.fixedDeltaTime
                    );
        if (experience >= levelExpRequired) {
            LevelUp();
        }
    }

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
        damage += 1;
        moveSpeed += 0.1f;
        // Gui Changes
        levelText.text = "Lv. " + level;
        // New XP required
        levelExpRequired = Mathf.FloorToInt(startExp * Mathf.Pow(curLevel, difficulty));
    }

    void ApplyDamage(int damage) {
        health -= damage;
        Flash();
    }

    // On Enemy death, they will send this to us to trigger XP for Jon
    void GiveEXP(int exp) {
        experience += exp;
    }

    public IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
