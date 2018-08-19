using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern/Rotation/Rotate")]
class Rotate : AttackPattern
{
    public float Angle;

    public override IEnumerator SequenceCoroutine(MonoBehaviour runner, Action callback,
        Vector3? positionOffset = default(Vector3?), Quaternion? rotationOffset = default(Quaternion?))
    {
        runner.transform.Rotate(new Vector3(0, 0, 1), Angle);
        callback();
        yield return null;
    }
}

