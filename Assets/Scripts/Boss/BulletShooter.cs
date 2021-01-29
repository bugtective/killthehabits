using UnityEngine;

public class BulletShooter : AttackPattern
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

    [SerializeField] private AudioSource _shootAudioSource = default;

    private int _currentProjectileAmount = 0;
    private int _currentAttackRound = 0;

    private void OnEnable()
    {
        _currentProjectileAmount = 0;
        _currentAttackRound = 0;
        _timer.StartCountDown(_projectileTimeInterval, Shoot);
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }
    
    private void Shoot()
    {
        var poolObject = _playBulletsPool.GetObject();
        poolObject.GetComponent<PlayBullet>().Shoot(transform.position, _target);
        _shootAudioSource.Play();

        _currentProjectileAmount++;
        
        if (_currentProjectileAmount >= _amountPerRound)
        {
            // New Round
            _currentAttackRound++;
            
            if (_currentAttackRound >= _rounds)
            {
               FinishAttack();
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