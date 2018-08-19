using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound : MonoBehaviour {

	public AudioClip clip;
	public string name;
	[Range(0f,1f)]
	public float volume;
	[Range(.1f,8f)]
	public float pitch;

	[HideInInspector]
	public AudioSource source;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
