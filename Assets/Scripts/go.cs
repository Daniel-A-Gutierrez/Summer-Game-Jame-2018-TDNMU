using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class go : MonoBehaviour {
	public float speed;
	public float timeout;
	public int damage;
	float start;
	// Use this for initialization
	void Start ()
	{
		start = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += transform.up.normalized*speed*Time.deltaTime;
		if(Time.time - start> timeout)
		{
			Destroy(gameObject);
		}
	}
}
