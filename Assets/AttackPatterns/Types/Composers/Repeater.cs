using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern/Composers/Repeater")]
public class Repeater : AttackPattern
{
    [Expandable]
    public AttackPattern Pattern;
    public int RepeatCount;
    public float Delay;

    private bool wait = false;
    public override IEnumerator SequenceCoroutine(MonoBehaviour runner, Action callback,
        Vector3? positionOffset = default(Vector3?), Quaternion? rotationOffset = default(Quaternion?))
    {
        for(int i = 0; i < RepeatCount; i++)
        {
            wait = true;
            runner.StartCoroutine(Pattern.SequenceCoroutine(runner, internalCallback, positionOffset, rotationOffset));
            yield return new WaitUntil(() => !wait);
            yield return new WaitForSeconds(Delay);
        }
        callback();
    }

    private void internalCallback()
    {
        wait = false;
    }
}
