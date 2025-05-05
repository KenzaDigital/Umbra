using UnityEngine;
using System;
using Unity.VisualScripting;

public class audioManager : MonoBehaviour
{
    /// This script is used to manage audio in the game

    //*---------------------------------------------Variables---------------------------------------------*//

    /// array of sounds for music and sfx
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    // this is a singleton instance of the audioManager
    public static audioManager instance;

    //---------------------------------------------Functions---------------------------------------------//

    public void Awake()
    {
        // if there is no instance of audioManager, set this as the instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("BackgroundMusic");
    }

    public void PlayMusic(string name, bool loop = false)
    {
        Sound s = Array.Find(musicSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.loop = loop; // Set looping based on the parameter
            musicSource.Play();
        }
    }



    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            sfxSource.clip = s.clip;
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void ToggleSound()
    {
        musicSource.mute = !musicSource.mute;
        sfxSource.mute = !sfxSource.mute;
    }

}