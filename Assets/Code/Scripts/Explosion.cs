using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioSource source;
    public AudioClip EXPLOSION;

    private void Start()
    { 
        source.PlayOneShot(EXPLOSION, 1f);
    }
}
   
