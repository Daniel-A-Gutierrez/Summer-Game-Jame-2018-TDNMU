using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SequnceUnit
{
    [Expandable]
    public AttackPattern pattern;
    public float delay;
}

[CreateAssetMenu(menuName = "Attack Pattern/Composers/Sequence")]
public class Sequence : AttackPattern
{
    public SequnceUnit[] Patterns;

    private bool wait = false;
    public override IEnumerator SequenceCoroutine(MonoBehaviour runner, Action callback,
        Vector3? positionOffset = default(Vector3?), Quaternion? rotationOffset = default(Quaternion?))
    {
        foreach(SequnceUnit su in Patterns)
        {
            wait = true;
            runner.StartCoroutine(su.pattern.SequenceCoroutine(runner, internalCallback, positionOffset, rotationOffset));
            yield return new WaitUntil(() => !wait);
            yield return new WaitForSeconds(su.delay);
        }
        callback();
    }

    private void internalCallback()
    {
        wait = false;
    }
}
