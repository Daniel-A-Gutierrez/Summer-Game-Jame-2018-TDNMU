using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern/Rotation/Rotate")]
class Rotate : AttackPattern
{
    public float Angle;
    public float Speed;
    public float Sensitivity = .1f;

    public override IEnumerator SequenceCoroutine(MonoBehaviour runner, Action callback, 
        Vector3? positionOffset = default(Vector3?), Quaternion? rotationOffset = default(Quaternion?))
    {
        float target = runner.transform.eulerAngles.x + Angle;
        float diff; 
        diff = (runner.transform.eulerAngles.x - target) % 360; 

        while(Mathf.Abs(diff) > Sensitivity)
        {
            Vector3 newEulerAngles = runner.transform.eulerAngles;
            newEulerAngles.x =
                Mathf.Lerp(
                    runner.transform.rotation.eulerAngles.x,
                    target,
                    Speed * Time.fixedDeltaTime
                    );
            runner.transform.eulerAngles = newEulerAngles;
            yield return new WaitForFixedUpdate();
            diff = (runner.transform.eulerAngles.x - target) % 360; 
        }
        callback();
    }
}

