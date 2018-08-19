using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern/Rotation/Rotate To Player")]
class RotateToPlayer : AttackPattern
{
    public override IEnumerator SequenceCoroutine(MonoBehaviour runner, Action callback,
        Vector3? positionOffset = default(Vector3?), Quaternion? rotationOffset = default(Quaternion?))
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player)
            //runner.transform.LookAt(player.transform, Vector3.forward);
            runner.transform.up = new Vector3(-runner.transform.position.x +
                player.transform.position.x, -runner.transform.position.y +
                player.transform.position.y, 0).normalized;
        callback();
        yield return null;
    }
}

