using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    private new AudioManager audio;
    private MusicManager music;

    private void Start()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        audio = FindObjectOfType<AudioManager>();
        music = audio.GetComponentInChildren<MusicManager>();
    }

    public void ToggleFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void ChangeMusicVolume(float value)
    {
        music.musicSource.volume = value;
    }

    public void ChangeSFXVolume(float value)
    {
        audio.globalVolumeMultiplier = value;
    }
}
