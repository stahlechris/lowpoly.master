using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_SFX : MonoBehaviour
{
    public AudioSource SFXSource;
    public AudioSource MusicSource;

    public static NPC_SFX Instance = null;

    private void Awake()
    {
        //If there is not already an instance of PlayerSFX, set it to this
        if (Instance == null)
        {
            Instance = this;
        }
        //else If there is already an instance of PlayerSFX, destroy it because there can only be one highlander.
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        //Make the PlayerSFX not destroyed when scenes change or reload.
        DontDestroyOnLoad(this.gameObject);
    }
    //Play a clip through the SFX source.
    public void PlaySFX(AudioClip clipToPlay, float volume, float pitch, bool fadeAudio)
    {
        SFXSource.clip = clipToPlay;
        SFXSource.volume = volume;
        SFXSource.pitch = pitch;
        if (!fadeAudio)
        {
            SFXSource.Play();
        }
        else
        {
            StartCoroutine(FadeAudio());
        }
    }

    //Play a clip through the music source.
    public void PlayMusic(AudioClip clipToPlay)
    {
        MusicSource.clip = clipToPlay;
        MusicSource.Play();
    }

    IEnumerator FadeAudio()
    {
        float startVolume = SFXSource.volume;
        SFXSource.Play();
        while (SFXSource.volume > 0)
        {
            SFXSource.volume -= startVolume * Time.deltaTime / 2f; //2.5 is fade time
            yield return null;
        }

    }
}
