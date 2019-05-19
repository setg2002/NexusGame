using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] MenuMusic = new AudioClip[2];

    private int musicNumber = 0;

    void Start()
    {
        StartCoroutine(MusicLoop());
    }

    void Update()
    {
        if (musicNumber == 1f)
        {
            if(!source.isPlaying)
            {
                source.PlayOneShot(MenuMusic[musicNumber], 1f);
                musicNumber++;
            }
        }
        if (musicNumber == 2f)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(MenuMusic[musicNumber], 1f);
                musicNumber--;
            }
        }
    }

    IEnumerator MusicLoop()
    {
        source.PlayOneShot(MenuMusic[0], 1f);
        yield return new WaitForSeconds(MenuMusic[0].length);
        musicNumber++;
    }
}
