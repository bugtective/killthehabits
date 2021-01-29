using UnityEngine;
using System;

public class AttackPattern : MonoBehaviour
{
    protected Timer _timer = new Timer();
    protected Transform _target = default;
    protected Action  _onFinishCallback = default;

    private void Awake()
    {
        enabled = false;
        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }

    public void Activate(Transform target, Action finishCallback)
    {
        _target = target;
        _onFinishCallback = finishCallback;
        enabled = true;
    }

    protected virtual void FinishAttack()
    {
        _onFinishCallback?.Invoke();
        enabled = false;
    }

    public virtual void StopAttacks()
    {
        enabled = false;
    }
}