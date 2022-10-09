using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    public Animator blood;

    public AudioSource musicPlayer;
    public AudioClip audios;

    // Start is called before the first frame update
    void Start()
    {
        //play blood blast sound D:
        //also destory gameobject before animation play 2nd time
        musicPlayer.clip = audios;
        musicPlayer.Play();

        Destroy(gameObject, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
