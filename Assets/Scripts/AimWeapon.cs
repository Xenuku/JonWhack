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
        aimTransform = transform.Find("Aim");
        Vector2 cursorPos = new Vector2(crosshair.width / 2, crosshair.height / 2);
        SetCursor(crosshair, cursorPos);
    }
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
        if (!PauseMenu.gameIsPaused) {
            if(upgradeChosen) {
                mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                HandleAim();
                HandleShooting();
                elapsedTime += Time.deltaTime;
            }
        }
    }

    private void HandleAim()
    {

        Vector3 aimDirection = (mouseP - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

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
                pBullet.GetComponent<PlayerBullet>().knockDirection = direction;

                carmera.SendMessage("shake");
                carmera.SendMessage("shake");
                carmera.SendMessage("shake");

                musicPlayer.clip = audios;
                musicPlayer.Play();
            }
        }

    }
}
