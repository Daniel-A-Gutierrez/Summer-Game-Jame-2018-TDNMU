using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsistentMovement : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;

    void Start()
    {
        Application.targetFrameRate = 60;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update ()
    {
        rb.velocity = new Vector3(0.0f, speed, 0.0f);
	}
}
