using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip EndCredits;
    private void Awake()
    {
        source.PlayOneShot(EndCredits, 1f);
    }
}
