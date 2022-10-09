using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    private float timeElapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        transform.position += new Vector3(0, 1.0f, 0) * Time.deltaTime;

        if (timeElapsed >= 2.0f)
        {
            Destroy(gameObject);
        }
    }
}
