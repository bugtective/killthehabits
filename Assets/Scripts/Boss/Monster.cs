using UnityEngine;
using System.Collections.Generic;

public class Monster : MonoBehaviour
{
    private enum MonsterState
    {
        None            = 0,
        Appearing,
        Idle,
        Preparing,
        MovingToCenter,
        Lunging,
        ShootingPlays,
        SpawningBombs,
        //SpawningTVs,
        TotalStates     = 8,
       
    }

    [SerializeField] private Character _character = default;

    [SerializeField] private BulletShooter _bulletShooter = default;
    [SerializeField] private LungeAttack _lungeAttack = default;
    [SerializeField] private SpawnBombs _spawnBombs = default;
    // [SerializeField] private SpawnTVsAttack _spawnTVs = default;

    [SerializeField] private Animator _animator = default;
    [SerializeField] private BossAnimationEvents _bossAnimationEvents = default;

    private Timer _timer = new Timer();

    private MonsterState _currentState = MonsterState.None;
    private MonsterState _lastAttack = MonsterState.None;

    [SerializeField] private float _dissapearingDuration = 0.7f;

    private SpriteRenderer _spriteRenderer = default;
    private Color _originalColor = default;
    private bool _dissapearing = false;
    private float _alphaTime = 0f;
    private float _alphaValue = 0f;
    private MonsterState _dissapearingState = MonsterState.None;

    private GameManager _gameManager = default;

    private List<MonsterState> _firstAttacks = new List<MonsterState>();

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.sharedMaterial.color;
        
        _bossAnimationEvents.OnAppearingEnding += OnAttackFinished;
        _bossAnimationEvents.OnPreparingEnding += StartAttack;

        _gameManager = GameManager.Instance;
    }

    public void AwakeBoss()
    {
        ChangeState(MonsterState.Appearing);
        transform.position = Vector3.zero;

        _firstAttacks.Clear();
        _firstAttacks.Add(MonsterState.ShootingPlays);
        _firstAttacks.Add(MonsterState.Lunging);
        _firstAttacks.Add(MonsterState.SpawningBombs);
        
        _spriteRenderer.sharedMaterial.color = _originalColor;
        _dissapearingState = MonsterState.None;
        _dissapearing = false;
        _alphaTime = 0f;
        _alphaValue = 0f;
    }

    private void OnDisable()
    {
        // HACK: this is so the Material asset doesn´t get changed after play
        // To fix this I should have a Materials manager or something like that.
        _spriteRenderer.sharedMaterial.color = _originalColor;
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);

        if (_currentState == MonsterState.MovingToCenter)
        {
            var finish = false;
            _alphaTime += Time.deltaTime;
            
            if (_dissapearing)
            {
                _alphaValue = Mathf.Lerp(1f, 0f, _alphaTime / _dissapearingDuration);
                
                if (_alphaTime >= _dissapearingDuration) {
                    _alphaValue = 0f;
                    _alphaTime = 0f;
                    _dissapearing = false;
                    
                    // Back to position
                    transform.position = Vector3.zero;
                }
            }
            else
            {
                _alphaValue = Mathf.Lerp(0f, 1f, _alphaTime / _dissapearingDuration);

                if (_alphaTime >= _dissapearingDuration) {
                    _alphaValue = 1f;
                    finish = true;
                }
            }

            _spriteRenderer.sharedMaterial.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _alphaValue);

            if (finish)
                ChangeState(_dissapearingState);
        }
    }

    private void ChangeState(MonsterState newState)
    {
        _currentState = newState;
        
        // Debug.Log($"--- Mode: {_currentState.ToString()} ---");

        switch (_currentState)
        {
            case MonsterState.None:
            {
                _lungeAttack.StopAttacks();
                _bulletShooter.StopAttacks();
                _spawnBombs.StopAttacks();
                // _spawnTVs.StopAttacks();

                if (_dissapearing) {
                    _dissapearingState = MonsterState.Idle;
                }
            }
            break;

            case MonsterState.Appearing:
            {
                _animator.Play("BossAppearing");
            }
            break;

            case MonsterState.Idle:
            {
                _animator.Play("BossIdle");

                if (!_gameManager.GameFinished)
                {
                    _timer.StartCountDown(Random.Range(1f, 3f), () => {
                        
                        if (!_gameManager.GameFinished)
                        {
                            ChangeState(MonsterState.Preparing);
                        }
                    });
                }
            }
            break;

            case MonsterState.Preparing:
            {
                _animator.Play("BossPreparing");
            }
            break;

            case MonsterState.Lunging:
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
                if (transform.position != Vector3.zero)
                {
                    StartDissapearing(MonsterState.SpawningBombs);
                }
                else
                {
                    _spawnBombs.Activate(_character.transform, OnAttackFinished);
                }
            }
            break;

            // case MonsterState.SpawningTVs:
            // {
            //     if (transform.position != Vector3.zero)
            //     {
            //        StartDissapearing(MonsterState.SpawningTVs);
            //     }
            //     else
            //     {
            //         _spawnTVs.Activate(_character.transform, OnAttackFinished);
            //     }
            // }
            // break;
        }
    }

    private void OnAttackFinished()
    {
        ChangeState(MonsterState.Idle);
    }

    private void StartAttack()
    {
        if (_gameManager.GameFinished)
        {
            ChangeState(MonsterState.None);
        }
        else
        {
            _animator.Play("BossAttack");

            var newAttack = MonsterState.None;

            if (_firstAttacks.Count > 0)
            {
                newAttack = _firstAttacks[0];
                _firstAttacks.RemoveAt(0);
            }
            else
            {
                do
                {
                    newAttack = (MonsterState)Random.Range((int)MonsterState.Lunging, (int)MonsterState.TotalStates);
                }
                while(newAttack == _lastAttack);
            }
            
            _lastAttack = newAttack;
            ChangeState(newAttack);
        }
    }

    private void StartDissapearing(MonsterState stateToGoAfter)
    {
        _currentState = MonsterState.MovingToCenter;
        _dissapearingState = stateToGoAfter;
        _dissapearing = true;
        _alphaTime = 0f;
    }

    public void OnGameEnd(bool playerWon)
    {
        ChangeState(MonsterState.None);
    }
}