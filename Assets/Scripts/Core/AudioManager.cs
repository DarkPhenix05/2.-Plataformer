using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{   
    public static AudioManager instance { get; private set;}
    private AudioSource audioSource;
    private AudioSource musicSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        if (instance == null ) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this) 
        {
            Destroy(gameObject);
        }

        ChangeVolume(0);
        MusicVolume(0);
    }

    public void PlaySound(AudioClip _sound)
    {
        audioSource.PlayOneShot(_sound);
    }

    public void ChangeVolume(float _change)
    {
        ChangeSourceVolume(1, "ChangeVol", _change, audioSource);
    }
    public void MusicVolume(float _change)
    {
        ChangeSourceVolume(.3f, "MusicVol", _change, musicSource);
    }
    private void ChangeSourceVolume(float baseVol, string volName, float change, AudioSource source)
    {
        float currentVolume = PlayerPrefs.GetFloat(volName, 1);
        currentVolume += change;
        if (currentVolume > 1)
        {
            currentVolume = 0;
        }
        else if (currentVolume < 0)
        {
            currentVolume = 1;
        }
        float finalVol = currentVolume * baseVol;
        source.volume = finalVol;

        PlayerPrefs.SetFloat(volName, currentVolume);
    }
}
