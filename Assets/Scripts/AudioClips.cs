using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClips : MonoBehaviour
{
    public static AudioClips instance;

    public AudioSource source;

    public AudioClip[] luggageHit;
    public AudioClip impact;

    void Awake()
    {
        instance = this;
    }
}