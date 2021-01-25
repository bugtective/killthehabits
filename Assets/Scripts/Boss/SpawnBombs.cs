using UnityEngine;

public class SpawnBombs : AttackPattern
{
    [SerializeField] private ObjectPool _bombsPool = default;
    [SerializeField] private SpawnPoints _spawnPoints = default;

    [SerializeField] private float _startDelay = 0.5f;
    [SerializeField] private float _timeBetweenRounds = 3f;
    
    [SerializeField] private int _rounds = 3;

    private int _currentRound = 0;

    private void OnEnable()
    {
        _currentRound = 0;
        _timer.StartCountDown(_startDelay, DropBombs);
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void DropBombs()
    {
        var points = _spawnPoints.GetSpawnPoints();

        foreach (var point in points)
        {
            var poolObject = _bombsPool.GetObject();
            poolObject?.GetComponent<NextBomb>().Spawn(point.position);
        }
        
        _currentRound++;
        
        if (_currentRound >= _rounds)
        {
            // HACK: Adding bombs explosion time to finish the attack
            _timer.StartCountDown(2f, FinishAttack);
        }
        else
        {   
            _timer.Reset(_timeBetweenRounds);
        }
    }
}