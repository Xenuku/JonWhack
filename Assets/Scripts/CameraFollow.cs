using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    [Range(1f,40f)] public float laziness = 1f;
    public bool lookAtTarget = true;
    public bool takeOffsetFromInitialPos = true;
    public Vector3 generalOffset;
    Vector3 whereCameraShouldBe;

    private Vector3 carmeraPos;

    private void Start() {
        if (takeOffsetFromInitialPos && target != null) generalOffset = transform.position - target.position;
    }

    void FixedUpdate()
    {

        whereCameraShouldBe = target.position + generalOffset;
        transform.position = Vector3.Lerp(transform.position, whereCameraShouldBe, 1 / laziness);

        if (lookAtTarget) transform.LookAt(target);

    }

    //screen shake effect
    public IEnumerator shake()
    {
        //random generate a set of random coordinates with in short range of current position
        carmeraPos.x = Random.Range(Camera.main.transform.position.x - 0.07f, Camera.main.transform.position.x + 0.07f);
        carmeraPos.y = Random.Range(Camera.main.transform.position.y - 0.07f, Camera.main.transform.position.y + 0.07f);
        carmeraPos.z = -20.0f;
        //let new position take effects
        transform.position = carmeraPos;

        //wait 0.2 seconds
        yield return new WaitForSeconds(0.2f);

        //go back to follow player after wait
        whereCameraShouldBe = target.position + generalOffset;
        transform.position = Vector3.Lerp(transform.position, whereCameraShouldBe, 1 / laziness);

        if (lookAtTarget) transform.LookAt(target);
    }
}
