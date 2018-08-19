using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPattern : ScriptableObject {
    public abstract IEnumerator SequenceCoroutine(MonoBehaviour runner, Action callback);
}
