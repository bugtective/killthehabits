using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class TVPatternAttack
{
    public int[] spawnPointsIdx;
    public string[] directions;
}

public class SpawnTVsAttack : AttackPattern
{
    [SerializeField] private ObjectPool _tvsPool = default;
    [SerializeField] private SpawnPoints _spawnPoints = default;

    [SerializeField] private float _startDelay = 0.5f;
    
    [Tooltip("Spawn Points Patterns")]
    [SerializeField] private List<TVPatternAttack> _patterns = default;

    private void OnEnable()
    {
        _timer.StartCountDown(_startDelay, SpawnTVs);
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void SpawnTVs()
    {
        int randomIdx = UnityEngine.Random.Range(0, _patterns.Count);

        var points = _spawnPoints.GetSpawnPoints(_patterns[randomIdx].spawnPointsIdx);
        

        for (int i = 0; i < points.Count; i++)
        {
            var poolObject = _tvsPool.GetObject();

            var dir = (RayTV.RayDirection) Enum.Parse(typeof(RayTV.RayDirection),_patterns[randomIdx].directions[i], true);

            poolObject?.GetComponent<RayTV>().Spawn(points[i].position, dir);
        }
        
        // HACK: Adding bombs explosion time to finish the attack
        _timer.StartCountDown(2f, FinishAttack);
    }
}