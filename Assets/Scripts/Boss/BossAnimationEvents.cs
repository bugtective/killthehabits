using UnityEngine;
using System;

public class BossAnimationEvents : MonoBehaviour 
{
    public event Action OnAppearingEnding = default;


    public void ApperaingEnding()
    {
        OnAppearingEnding?.Invoke();
    }
}