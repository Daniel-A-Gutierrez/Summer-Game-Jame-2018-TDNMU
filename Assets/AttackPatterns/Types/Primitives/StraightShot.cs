﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern/Primatives/Straight Shot")]
public class StraightShot : AttackPattern
{
    public BulletPoolType pool;
    public int Speed;
    public int LifeTime;

    public override IEnumerator SequenceCoroutine(MonoBehaviour runner, Action callback,
        Vector3? positionOffset = default(Vector3?), Quaternion? rotationOffset = default(Quaternion?))
    {
        GameObject g = pool.GetBullet();
        Vector3 actualPositionOffset = positionOffset ?? new Vector3();
        actualPositionOffset = runner.transform.rotation * actualPositionOffset;
        g.transform.position = runner.transform.position + actualPositionOffset; 
        g.transform.rotation = runner.transform.rotation;
        callback();

        float endTime = Time.time + LifeTime;
        while(Time.time < endTime)
        {
            g.transform.position += Speed * g.transform.forward * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        pool.ReturnBullet(g);
    }
}
