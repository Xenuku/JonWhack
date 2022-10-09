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
        musicPlayer.clip = audios;
        musicPlayer.Play();

        Destroy(gameObject, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
