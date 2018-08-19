using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public float backgroundSize;
    public float paralaxSpeed;

    private GameObject player;

    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 3;
    private int downIndex;
    private int upIndex;
    private float lastCameraY;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("QuinSpriteFinal_1");
        cameraTransform = Camera.main.transform;
        lastCameraY = cameraTransform.position.y;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }
        downIndex = 0;
        upIndex = layers.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (player.GetComponent<Doggo>().introRunning == false)
        //{
        float deltaX = cameraTransform.position.y - lastCameraY;
        transform.position += Vector3.up * (deltaX * paralaxSpeed);
        lastCameraY = cameraTransform.position.y;
        if (cameraTransform.position.y < (layers[downIndex].transform.position.y + viewZone))
        {
            ScrollDown();
        }

        if (cameraTransform.position.y > (layers[upIndex].transform.position.y - viewZone))
        {
            ScrollUp();
        }
        //}
    }

    private void ScrollDown()
    {
        int lastRight = upIndex;
        layers[upIndex].position = Vector3.up * (layers[downIndex].position.y - backgroundSize);
        downIndex = upIndex;
        upIndex--;
        if (upIndex < 0)
            upIndex = layers.Length - 1;
    }

    private void ScrollUp()
    {
        int lastLeft = downIndex;
        layers[downIndex].position = Vector3.up * (layers[upIndex].position.y + backgroundSize);
        upIndex = downIndex;
        downIndex++;
        if (downIndex == layers.Length)
            downIndex = 0;
    }
}
