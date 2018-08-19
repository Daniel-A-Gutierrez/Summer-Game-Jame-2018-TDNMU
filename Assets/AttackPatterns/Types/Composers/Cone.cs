using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern/Composers/Cone")]
public class Cone : AttackPattern
{
    [Expandable]
    public AttackPattern Pattern;
    public int RepeatCount;
    public int Angle;

    private int wait;
    public override IEnumerator SequenceCoroutine(MonoBehaviour runner, Action callback,
        Vector3? positionOffset = default(Vector3?), Quaternion? rotationOffset = default(Quaternion?))
    {
        wait = 0;
        float anglePerRepeat = (float)Angle / RepeatCount;
        runner.transform.Rotate(new Vector3(0, 0, -1), -(float)Angle / 2 + anglePerRepeat / 2);
        for (int i = 0; i < RepeatCount; i++)
        {
            runner.StartCoroutine(Pattern.SequenceCoroutine(runner, internalCallback, positionOffset, rotationOffset));
            runner.transform.Rotate(new Vector3(0, 0, -1), anglePerRepeat);
        }
        runner.transform.Rotate(new Vector3(0, 0, -1), -(float)Angle / 2 - anglePerRepeat / 2);
        yield return new WaitUntil(() => wait >= RepeatCount - 1);
        callback();
    }

    private void internalCallback()
    {
        wait++;
    }
}
