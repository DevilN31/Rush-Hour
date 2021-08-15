using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {

    protected SoundManager() { } // guarantee this will be always a singleton only - can't use the constructor!

    public AudioClip BrakeSound;
    public AudioClip SwipeUpDown;
    public AudioClip SwipeLeftRight;

    public AudioClip hornSound1;
    public AudioClip hornSound2;
    public AudioClip hornSound3;

    public AudioClip carHitSound;

    public AudioClip ambulanceSiren;
    public AudioClip policeSiren;

    public AudioClip carFinalHit;

    public AudioClip buttonClick;

    private AudioSource audioSource;

    public AudioSource bgMusicAudioSource;
    public AudioSource sirenAudioSource;

    public bool canPlayBrakeSound = true;

    public float effectsVolume = 0.5f;
    void Awake()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }
    public void PlayBrakeSound()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.GameOver)
            return;

        if (canPlayBrakeSound)
        {
            audioSource.PlayOneShot(BrakeSound , effectsVolume);
            canPlayBrakeSound = false;
        }
    }

    public void PlaySwipeUpDown()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.GameOver)
            return;
        audioSource.PlayOneShot(SwipeUpDown , effectsVolume);
    }

    public void PlaySwipeLeftRight()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.GameOver)
            return;
        audioSource.PlayOneShot(SwipeLeftRight , effectsVolume);
    }

    public void PlayHorn()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.GameOver)
            return;

        int val = Random.Range(0, 3);
        if (val == 0)
            audioSource.PlayOneShot(hornSound1 , effectsVolume);
        else if (val == 1)
            audioSource.PlayOneShot(hornSound2, effectsVolume);
        else
            audioSource.PlayOneShot(hornSound3, effectsVolume);
    }

    public void PlayCarHitMirror()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.GameOver)
            return;

        audioSource.PlayOneShot(carHitSound , effectsVolume);
    }

    public void PlayAmbulanceSiren()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.GameOver || sirenAudioSource.isPlaying)
        {
            return;
        }

        sirenAudioSource.clip = ambulanceSiren;
        sirenAudioSource.Play();
    }

    public void PlayPoliceSiren()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.GameOver || sirenAudioSource.isPlaying)
        {
            return;
        }

        sirenAudioSource.clip = policeSiren;
        sirenAudioSource.Play();
    }

    public void StopMusic()
    {
        if(sirenAudioSource.isPlaying)
        sirenAudioSource.Stop();
        //bgMusicAudioSource.Stop();
    }

    public void StartMusic()
    {
        bgMusicAudioSource.Play();
    }

    public void PlayCarFinalHit()
    {
        audioSource.PlayOneShot(carFinalHit , effectsVolume);
    }

    public void PlayButtonPress()
    {
        audioSource.PlayOneShot(buttonClick , effectsVolume);
    }
    
}
