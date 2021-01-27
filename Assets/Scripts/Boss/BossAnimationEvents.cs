using UnityEngine;
using System;

public class BossAnimationEvents : MonoBehaviour 
{
    public event Action OnAppearingEnding = default;
    public event Action OnPreparingEnding = default;

    public void ApperaingEnding()
    {
        OnAppearingEnding?.Invoke();
    }

    public void PreparingEnding()
    {
        OnPreparingEnding?.Invoke();
    }
}