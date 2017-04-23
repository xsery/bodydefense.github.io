﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Manages all the sounds, inherits from singleton to make the class into a singleton
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    /// <summary>
    /// A reference to the audios source
    /// </summary>
    [SerializeField]
    private AudioSource musicSource;

    /// <summary>
    /// A reference to the sfx Audiosource
    /// </summary>
    [SerializeField]
    private AudioSource sfxSource;

    /// <summary>
    /// A reference to the sfxSlider
    /// </summary>
    [SerializeField]
    private Slider sfxSlider;

    /// <summary>
    /// A reference to the musicSlider
    /// </summary>
    [SerializeField]
    private Slider musicSlider;

    /// <summary>
    /// A dictionary for all the audio clips
    /// </summary>
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    // Use this for initialization
    void Start()
    {
        //Instantiates the audioclips array by loading all audioclips from the assetfolder
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        //Stores all the audio clips
        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }

        //Laods the saved volumes
        LoadVolume();

        //Makes the updatevolume listen to the musicsliders
        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });


    }

    /// <summary>
    /// Plays an sfx sound
    /// </summary>
    /// <param name="name"></param>
    public void PlaySFX(string name)
    {
        //Plays the clip once
        sfxSource.PlayOneShot(audioClips[name]);
    }

    /// <summary>
    /// Updates the volumes according to the sliders
    /// </summary>
    private void UpdateVolume()
    {
        //Sets the music volume
        musicSource.volume = musicSlider.value;

        //Sets the sfx volume
        sfxSource.volume = sfxSlider.value;

        //Saves the values
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }

    /// <summary>
    /// Loads the volumes 
    /// </summary>
    private void LoadVolume()
    {
        //Loads the sfx volume
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.5f);
        //Loads the muisc volumes
        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.5f);

        //Updates the sliders
        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;
    }
}
