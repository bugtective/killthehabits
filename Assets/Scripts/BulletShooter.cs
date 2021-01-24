using UnityEngine;
using System;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private ObjectPool _playBulletsPool = default;
 
    [Tooltip("Amount of projectiles per fire round")]
    [SerializeField] private int _amountPerRound = 3;
 
    [Tooltip("Amount of rounds per attack state")]
    [SerializeField] private int _rounds = 3;
 
    [Tooltip("Time interval between projectile")]
    [SerializeField] private float _projectileTimeInterval = 0.3f;
 
    [Tooltip("Time interval between rounds")]
    [SerializeField] private float _roundTimeInterval = 1f;

    private int _currentProjectileAmount = 0;
    private int _currentAttackRound = 0;

    private Timer _timer = new Timer();

    private Transform _target = default;

    private Action  _onFinishCallback = default;

    private void Awake()
    {
        enabled = false;
    }

    public void Activate(Transform target, Action finishCallback)
    {
        _target = target;
        _onFinishCallback = finishCallback;
        enabled = true;
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void OnEnable()
    {
        _currentProjectileAmount = 0;
        _currentAttackRound = 0;
        _timer.StartCountDown(_projectileTimeInterval, Shoot);
    }

    private void Shoot()
    {
        var poolObject = _playBulletsPool.GetObject();
        poolObject.GetComponent<PlayBullet>().Shoot(_target);

        _currentProjectileAmount++;
        
        if (_currentProjectileAmount >= _amountPerRound)
        {
            // New Round
            _currentAttackRound++;
            
            if (_currentAttackRound >= _rounds)
            {
                _onFinishCallback?.Invoke();
                enabled = false;
            }
            else
            {
                _currentProjectileAmount = 0;
                _timer.Reset(_roundTimeInterval);
            }
        }
        else
        {
            _timer.Reset(_projectileTimeInterval);
        }
    }
}