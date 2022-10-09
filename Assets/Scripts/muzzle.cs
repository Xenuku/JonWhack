using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muzzle : MonoBehaviour
{
    private Vector3 mousePosition;
    public SpriteRenderer spriteRenderer;
    public Transform gunPoint;

    // Start is called before the first frame update
    void Start()
    {
        gunPoint = transform.Find("GunEndPoint");

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 localScale = new Vector3(0.05f, 0.05f, 0);

        transform.localScale = localScale;

        Destroy(gameObject, 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
