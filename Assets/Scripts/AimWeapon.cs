using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    Vector3 mouseP;
    protected float elapsedTime;
    public GameObject pBullet;
    public float shootRate = 0.3f;
    public float bulletSpeed = 60.0f;
    public GameObject endGunPoint;
    public Texture2D crosshair;
    private SpriteRenderer playerSprite;
    public GameObject player;
    public SpriteRenderer Gun;
    public SpriteRenderer Hand1;
    public SpriteRenderer Hand2;

    private Vector2 carmeraPos;
    public GameObject carmera;
    public GameObject upgradeManager;

    public AudioSource musicPlayer;
    public AudioClip audios;

    private bool upgradeChosen;

    private void Awake()
    {
        //setup cursor, references upon awake
        aimTransform = transform.Find("Aim");
        Vector2 cursorPos = new Vector2(crosshair.width / 2, crosshair.height / 2);
        SetCursor(crosshair, cursorPos);
    }
    // Set the cursor to a crosshair
    void SetCursor(Texture2D sprite, Vector2 center)
    {
        Cursor.SetCursor(sprite, center, CursorMode.Auto);
    }

    void Start()
    {
        elapsedTime = shootRate; // First shot can fire (thanks Dimitri)
        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        
    }

    private void Update()
    {
        upgradeChosen = upgradeManager.GetComponent<UpgradeManager>().upgradeChosen;
        shootRate = player.GetComponent<PlayerController>().fireRate;
        // only let the user aim and shoot if the game is not paused, and the weapon has been chosen
        if (!PauseMenu.gameIsPaused) {
            if(upgradeChosen) {
                mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                HandleAim();
                HandleShooting();
                elapsedTime += Time.deltaTime;
            }
        }
    }

    //adjust gun spriterender so player will not have a gun that upside down 
    private void HandleAim()
    {

        Vector3 aimDirection = (mouseP - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        // Flip the sprite depending which way the player is aiming their mouse
        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            Gun.flipY = true;
        }
        else
        {
            Gun.flipY = false;
        }
        
        aimTransform.localScale = localScale;
    }
    
    //generate bullet with speed according to mouse position
    private void HandleShooting()
    {
        if (Input.GetButton("Fire1"))
        {
            if (elapsedTime >= shootRate)
            {
                elapsedTime = 0.0f;
                Vector3 mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = (Vector3)((mouseP - transform.position));
                direction.Normalize();
                GameObject baseBullet = (GameObject)Instantiate(
                                    pBullet,
                                    endGunPoint.transform.position + (Vector3)(direction * 0.5f),
                                    Quaternion.identity);

                baseBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
                //send knockback direction to bullets script so they can apply kockback correctly to enemy
                pBullet.GetComponent<PlayerBullet>().knockDirection = direction;

                //shake screen 3 times upon shooting
                carmera.SendMessage("shake");
                carmera.SendMessage("shake");
                carmera.SendMessage("shake");

                //play gunshot sound
                musicPlayer.clip = audios;
                musicPlayer.Play();
            }
        }

    }
}
