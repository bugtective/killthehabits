using UnityEngine;

public class Monster : MonoBehaviour
{
    private enum MonsterState
    {
        None            = 0,
        Appearing       = 1,
        Idle            = 2,
        Preparing       = 3,
        Moving          = 4,
        ShootingPlays   = 5,
        SpawningBombs   = 6,
        SpawningTVs     = 7,
       
        TotalStates     = 8,
    }

    [SerializeField] private Character _character = default;

    [SerializeField] private BulletShooter _bulletShooter = default;
    [SerializeField] private LungeAttack _lungeAttack = default;
    [SerializeField] private SpawnBombs _spawnBombs = default;
    [SerializeField] private SpawnTVsAttack _spawnTVs = default;

    [SerializeField] private Animator _animator = default;
    [SerializeField] private BossAnimationEvents _bossAnimationEvents = default;

    private Timer _timer = new Timer();

    private MonsterState _currentState = MonsterState.None;

    private void Awake()
    {
        ChangeState(MonsterState.Appearing);
        _bossAnimationEvents.OnAppearingEnding += OnAttackFinished;
        _bossAnimationEvents.OnPreparingEnding += StartAttack;
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void ChangeState(MonsterState newState)
    {
        _currentState = newState;
        
        // Debug.Log($"--- Mode: {_currentState.ToString()} ---");

        switch (_currentState)
        {
            case MonsterState.Appearing:
            {
            }
            break;

            case MonsterState.Idle:
            {
                _animator.Play("BossIdle");
                _timer.StartCountDown(Random.Range(1f, 3f), () => {
                    ChangeState(MonsterState.Preparing);
                });
            }
            break;

            case MonsterState.Preparing:
            {
                _animator.Play("BossPreparing");
            }
            break;

            case MonsterState.Moving:
            {
                _lungeAttack.Activate(_character.transform, OnAttackFinished);
            }
            break;

            case MonsterState.ShootingPlays:
            {
                _bulletShooter.Activate(_character.transform, OnAttackFinished);
            }
            break;

            case MonsterState.SpawningBombs:
            {
                _spawnBombs.Activate(_character.transform, OnAttackFinished);
            }
            break;

            case MonsterState.SpawningTVs:
            {
                _spawnTVs.Activate(_character.transform, OnAttackFinished);
            }
            break;
        }
    }

    private void OnAttackFinished()
    {
        ChangeState(MonsterState.Idle);
    }

    private void StartAttack()
    {
        _animator.Play("BossAttack");
        ChangeState((MonsterState)Random.Range(4, (int)MonsterState.TotalStates));
    }
}