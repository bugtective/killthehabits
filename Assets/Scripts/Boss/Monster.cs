using UnityEngine;

public class Monster : MonoBehaviour
{
    private enum MonsterState
    {
        None,
        Appearing,
        Moving,
        ShootingPlays,
        SpawningBombs,
        SpawningTVs
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
                _timer.StartCountDown(1f, FinishAppearing);
            }
            break;

            case MonsterState.Moving:
            {
                _lungeAttack.Activate(_character.transform, FinishMoving);
            }
            break;

            case MonsterState.ShootingPlays:
            {
                _bulletShooter.Activate(_character.transform, FinishShooting);
            }
            break;

            case MonsterState.SpawningBombs:
            {
                _spawnBombs.Activate(_character.transform, FinishBombing);
            }
            break;

            case MonsterState.SpawningTVs:
            {
                _spawnTVs.Activate(_character.transform, FinishTVAttack);
            }
            break;
        }
    }

    private void FinishAppearing()
    {
        ChangeState(MonsterState.SpawningTVs);
    }

    private void FinishMoving()
    {
        ChangeState(MonsterState.ShootingPlays);
    }

    private void FinishShooting()
    {
        ChangeState(MonsterState.Moving);
    }

    private void FinishBombing()
    {
        ChangeState(MonsterState.Appearing);
    }

    private void FinishTVAttack()
    {
        ChangeState(MonsterState.Appearing);
    }
}