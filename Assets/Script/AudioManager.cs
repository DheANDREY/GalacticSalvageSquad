using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfx;
    public AudioClip move, hit; public AudioClip[] pick;

    //public static AudioManager instance;
    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    void Start()
    {
        sfx = GetComponent<AudioSource>();
    }

    public void PlaySFX_Hit()
    {
        sfx.Stop();
        sfx.pitch = 1;
        sfx.PlayOneShot(hit);
    }
    public void PlaySFX_Move()
    {
        sfx.pitch = 1;
        sfx.PlayOneShot(move);
    }
    public void StopSFX_Move()
    {        
        sfx.pitch = -2.7f;
        sfx.PlayOneShot(move);
    }
    public void SetSFXVolume(float volume)
    {
        sfx.volume = volume;
    }
}
