using UnityEngine;

public class Monster : MonoBehaviour
{
    private enum MonsterState
    {
        None            = 0,
        Appearing       = 1,
        Moving          = 2,
        ShootingPlays   = 3,
        SpawningBombs   = 4,
        SpawningTVs     = 5,
       
        TotalStates     = 6,
    }

    [SerializeField] private Character _character = default;

    [SerializeField] private BulletShooter _bulletShooter = default;
    [SerializeField] private LungeAttack _lungeAttack = default;
    [SerializeField] private SpawnBombs _spawnBombs = default;
    [SerializeField] private SpawnTVsAttack _spawnTVs = default;

    private Timer _timer = new Timer();

    private MonsterState _currentState = MonsterState.None;

    private void Awake()
    {
        ChangeState(MonsterState.Appearing);
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
                _timer.StartCountDown(1f, OnFinishState);
            }
            break;

            case MonsterState.Moving:
            {
                _lungeAttack.Activate(_character.transform, OnFinishState);
            }
            break;

            case MonsterState.ShootingPlays:
            {
                _bulletShooter.Activate(_character.transform, OnFinishState);
            }
            break;

            case MonsterState.SpawningBombs:
            {
                _spawnBombs.Activate(_character.transform, OnFinishState);
            }
            break;

            case MonsterState.SpawningTVs:
            {
                _spawnTVs.Activate(_character.transform, OnFinishState);
            }
            break;
        }
    }

    private void OnFinishState()
    {
        MonsterState newState = MonsterState.None;
        do
        {
            newState = (MonsterState)Random.Range(2, (int)MonsterState.TotalStates);
        }
        while (newState == _currentState);

        // For now, we simply randomize the behavior
        ChangeState(newState);
    }
}