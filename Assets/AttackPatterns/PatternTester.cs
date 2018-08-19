using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternTester : MonoBehaviour {
    public AttackPattern pattern;

	// Use this for initialization
	void Start () {
        Debug.Log("Start");
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Start Atack");
            StartCoroutine(pattern.SequenceCoroutine(this, Callback));
        }
    }

    void Callback()
    {
        Debug.Log("End");
    }
}
