using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern/Composers/Spread")]
public class Spread : AttackPattern
{
    [Expandable]
    public AttackPattern Pattern;
    public int RepeatCount;
    public float Distance;

    private int wait;
    public override IEnumerator SequenceCoroutine(MonoBehaviour runner, Action callback,
        Vector3? positionOffset = default(Vector3?), Quaternion? rotationOffset = default(Quaternion?))
    {
        wait = 0;
        float totalWidth = RepeatCount * Distance;
        Vector3 newOffset = positionOffset??new Vector3();
        newOffset.x -= totalWidth / 2;
        newOffset.x += Distance / 2;
        for (int i = 0; i < RepeatCount; i++)
        {
            runner.StartCoroutine(Pattern.SequenceCoroutine(runner, internalCallback, newOffset, rotationOffset));
            newOffset.x += Distance;
        } 
        yield return new WaitUntil(() => wait >= RepeatCount-1);
        callback();
    }

    private void internalCallback()
    {
        wait++;
    }
}
