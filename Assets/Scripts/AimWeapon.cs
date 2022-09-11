using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    Vector3 mouseP;
    protected float elapsedTime;
    public GameObject pBullet;
    public float shootRate = 0.5f;
    public float bulletSpeed = 30.0f;
    public GameObject endGunPoint;
    public Texture2D crosshair;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        SetCursor(crosshair, mouseP);
        
    }
    void SetCursor(Texture2D sprite, Vector2 center) {
        Cursor.SetCursor(sprite, center, CursorMode.Auto);
    }

    private void Update()
    {
        mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        HandleAim();
        HandleShooting();
        elapsedTime += Time.deltaTime;
        
    }

    private void HandleAim()
    {
        Vector3 aimDirection = (mouseP - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 localScale = Vector3.one;
        if(angle > 90 || angle < -90) {
            localScale.y = -1f;
        } else {
            localScale.y = +1f;
        }
        aimTransform.localScale = localScale;
    }

    private void HandleShooting()
    {
        if(Input.GetButton("Fire1")) 
            {
                if (elapsedTime >= shootRate) {
                    elapsedTime = 0.0f;
                    Vector3 mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = (Vector2)((mouseP - transform.position));
                    direction.Normalize ();
                    GameObject baseBullet = (GameObject)Instantiate (
                                        pBullet,
                                        endGunPoint.transform.position + (Vector3)(direction * 0.5f),
                                        Quaternion.identity);
                    baseBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
                }
            }
            
    }
}