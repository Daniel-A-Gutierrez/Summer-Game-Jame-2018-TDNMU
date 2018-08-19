using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern/Test")]
public class TestPattern : AttackPattern
{
    public string InitialMessage = "Hello World";
    public int WaitTime = 10;

    public override IEnumerator SequenceCoroutine(MonoBehaviour runner, Action callback,
        Vector3? positionOffset = null, Quaternion? rotationOffset = null)
    {
        Debug.Log(InitialMessage);
        for(int i = 0; i < WaitTime; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1.0f);
        }
        callback();
    }
}
