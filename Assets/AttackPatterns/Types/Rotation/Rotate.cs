using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern/Rotation/Rotate")]
class Rotate : AttackPattern
{
    public float Angle;
    public float Speed;

    public override IEnumerator SequenceCoroutine(MonoBehaviour runner, Action callback,
        Vector3? positionOffset = default(Vector3?), Quaternion? rotationOffset = default(Quaternion?))
    {
        float frameSpeed = Speed * Time.fixedDeltaTime;
        float frameCount = Angle / frameSpeed;

        for (int i = 0; i < frameCount; i++)
        {
            runner.transform.Rotate(new Vector3(0, 0, frameSpeed));
            yield return new WaitForFixedUpdate();
        }
        callback();
    }
}

