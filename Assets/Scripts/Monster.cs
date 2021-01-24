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
        
        //Debug.Log($"--- Mode: {_currentState.ToString()} ---");

        switch (_currentState)
        {
            case MonsterState.Appearing:
            {
                _timer.StartCountDown(1f, FinishAppearing);
            }
            break;

            case MonsterState.Moving:
            {
                _timer.StartCountDown(1f, FinishMoving);
            }
            break;

            case MonsterState.ShootingPlays:
            {
                _bulletShooter.Activate(_character.transform, FinishShooting);
            }
            break;
        }
    }

    private void FinishAppearing()
    {
        ChangeState(MonsterState.ShootingPlays);
    }

    private void FinishMoving()
    {
        ChangeState(MonsterState.ShootingPlays);
    }

    private void FinishShooting()
    {
        ChangeState(MonsterState.Moving);
    }
}