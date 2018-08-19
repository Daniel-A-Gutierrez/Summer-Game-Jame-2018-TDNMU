using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternTester : MonoBehaviour {
    [Expandable]
    public AttackPattern pattern;
    private bool nonRepeat = false;

	// Use this for initialization
	void Start () {
        Debug.Log("Start");
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !nonRepeat)
        {
            nonRepeat = true;
            Debug.Log("Start Atack");
            StartCoroutine(pattern.SequenceCoroutine(this, Callback));
        }
    }

    void Callback()
    {
        nonRepeat = false;
        Debug.Log("End");
    }
}
