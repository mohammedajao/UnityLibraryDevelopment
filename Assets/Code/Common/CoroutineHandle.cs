using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandle : IEnumerator
{
    public event Action<CoroutineHandle> Completed;
    public bool IsDone { get; private set; }
    public bool MoveNext() => !IsDone;
    public object Current { get; }
    public void Reset() {}

    public CoroutineHandle(MonoBehaviour owner, IEnumerator coroutine)
    {
        Current = owner.StartCoroutine(Wrap(coroutine));
    }

    private IEnumerator Wrap(IEnumerator coroutine)
    {
        yield return coroutine;
        IsDone = true;
        Completed?.Invoke(this);
    }
}
