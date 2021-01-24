using UnityEngine;
using System;

public class Timer
{
    private float _currentTime = 0f;
    private float _duration = 0f;
    private bool _active = false;
    
    private Action _OnCountdownEnd = default;

    public void Update(float dt)
    {
        if (!_active)
            return;

        _currentTime += dt;

        if (_currentTime >= _duration)
        {
            _active = false;
            _OnCountdownEnd?.Invoke();
        }
    }

    public void StartCountDown(float duration, Action callback)
    {
        _duration = duration;
        _OnCountdownEnd = callback;

        _currentTime = 0f;
        _active = true;
    }

    public void Reset(float duration = 0f)
    {
        _duration = duration != 0f ? duration : _duration;
        _currentTime = 0f;
        _active = true;
    }
}