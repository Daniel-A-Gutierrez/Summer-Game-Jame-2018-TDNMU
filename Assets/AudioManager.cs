using UnityEngine.Audio;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour {

	public Sound[] sounds;
	// Use this for initialization
	void Awake()
	{
		foreach(Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}
	//thanks brackeys
	public void Play(string name)
	{
		Sound s = Array.Find(sounds,sounds => sounds.name == name);
		s.source.Play();
	}

	public void Stop(string name)
	{
		Sound s = Array.Find(sounds,sounds => sounds.name == name);
		s.source.Stop();
	}

	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
