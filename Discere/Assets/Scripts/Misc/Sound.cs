using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {

	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	public bool loop = false;

	// Number of sources in use for this sound.
	[Range(1, 5)]
	public int nbSources = 1;

	public AudioMixerGroup mixerGroup;

	[HideInInspector]
	public List<AudioSource> sources;

	[HideInInspector]
	public int lastSource = 0;

}
