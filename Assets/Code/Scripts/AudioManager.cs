using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioSource AmbientLoop;
    public AudioClip[] VoiceLogs = new AudioClip[7];
    public AudioClip[] AmbientSounds = new AudioClip[3];
    private AudioClip rndmClip;

    private bool IsAlreadyRandom;

    public float volume;
    public int HighRange = 35;
    public int LowRange = 10;

    void Start()
    {
        AmbientLoop.Play();

        volume = 0.5f;
    }

    void Update()
    {
        AmbientLoop.volume = volume;
        rndmClip = AmbientSounds[Random.Range(0, AmbientSounds.Length)];
        if (!IsAlreadyRandom)
        {
            StartCoroutine(RandomTimeAndClip());
        }
    }

    IEnumerator RandomTimeAndClip()
    {
        IsAlreadyRandom = true;
        yield return new WaitForSeconds(Random.Range(LowRange, HighRange));
        source.PlayOneShot(rndmClip, .5f);

        yield return new WaitForSeconds(rndmClip.length);
        IsAlreadyRandom = false;
    }

    public void TriggerSound(AudioClip TriggerSound)
    {
        source.PlayOneShot(TriggerSound, volume);
    }

}
