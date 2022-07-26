using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 2f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        
        
        transform.Translate(Vector3.right * h);
        transform.Rotate(0, moveSpeed * v, 0);
    }
}
