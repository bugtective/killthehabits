using System.Collections.Generic;
using UnityEngine;

public class SpawnBombs : AttackPattern
{
    [SerializeField] private ObjectPool _bombsPool = default;
    [SerializeField] private SpawnPoints _spawnPoints = default;

    [SerializeField] private float _startDelay = 0.5f;
    [SerializeField] private float _timeBetweenRounds = 3f;
    
    [SerializeField] private int _rounds = 3;

    [Tooltip("Spawn Points Patterns")]
    [SerializeField] private List<string> _patterns = new List<string>();

    private int _currentRound = 0;

    private List<int[]> _patternsIdx = default;

    protected override void OnAwake()
    {
        _patternsIdx = new List<int[]>();

        // Parse Patterns
        foreach (var pattern in _patterns)
        {
            string[] subs = pattern.Split(',');
            int[] indexes = new int[subs.Length];
            for (int i = 0; i < subs.Length; i++)
            {
                indexes[i] = int.Parse(subs[i]);
            }

            _patternsIdx.Add(indexes);
        }
    }

    private void OnEnable()
    {
        _currentRound = 0;

        // Back to position
        transform.position = Vector3.zero;

        _timer.StartCountDown(_startDelay, DropBombs);
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void DropBombs()
    {
        var points = _spawnPoints.GetSpawnPoints(_patternsIdx);

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