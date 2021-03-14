using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
	public AudioMixerGroup mixerGroup;
	public float globalVolumeMultiplier = 1f;

	public Sound[] sounds;

	void Awake()
	{
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}

	}

    private void Start()
    {
    }

	public void Play(string sound, float volumeMultiplier = 1f, float pitchMultiplier = 1f)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * globalVolumeMultiplier * volumeMultiplier;
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f)) * pitchMultiplier;
		s.source.Play();
	}

	public void Stop(string sound)
    {
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

}
