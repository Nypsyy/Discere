using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
	public AudioMixerGroup mixerGroup;
	public float globalVolumeMultiplier = 1f;

	public Sound[] sounds;
	private Dictionary<string, Sound> soundsDict;

	void Awake()
	{
		soundsDict = new Dictionary<string, Sound>(sounds.Length);

		foreach (Sound s in sounds)
		{
			s.sources = new List<AudioSource>();
			for (int i = 0; i < s.nbSources; i++)
			{
				s.sources.Add(gameObject.AddComponent<AudioSource>());
				s.sources[i].clip = s.clip;
				s.sources[i].loop = s.loop;

				s.sources[i].outputAudioMixerGroup = mixerGroup;
			}

			soundsDict.Add(s.name, s);
		}

	}

    private void Start()
    {
    }

	public void Play(string sound, float volumeMultiplier = 1f, float pitchMultiplier = 1f)
	{
		if (!soundsDict.ContainsKey(sound))
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		Sound s = soundsDict[sound];

		s.lastSource = (s.lastSource + 1) % s.nbSources;

		s.sources[s.lastSource].volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * globalVolumeMultiplier * volumeMultiplier;
		s.sources[s.lastSource].pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f)) * pitchMultiplier;
		s.sources[s.lastSource].Play();
	}

	/**
	 * Stop a sound. An index corresponding to the audio source to stop can be specified.
	 * A negative index means "all sources"
	 */
	public void Stop(string sound, int index = -1)
    {
		if (!soundsDict.ContainsKey(sound))
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		Sound s = soundsDict[sound];
		if (index >= s.nbSources)
        {
			Debug.LogWarning("Sound: " + name + " : source " + index.ToString() + " not found!");
			return;
		}

		if (index < 0)
			for (int i = 0; i < s.nbSources; i++)
				s.sources[i].Stop();
		else
			s.sources[index].Stop();
	}

}
