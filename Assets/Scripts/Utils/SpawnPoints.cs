using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public List<Transform> GetSpawnPoints(List<int[]> pattersIdx)
    {
        List<Transform> points = new List<Transform>();

        int randomIdx = Random.Range(0, pattersIdx.Count);
        
        foreach (var i in pattersIdx[randomIdx])
        {
            points.Add(transform.GetChild(i));
        }

        return points;
    }

    public List<Transform> GetSpawnPoints(int[] pattern)
    {
        List<Transform> points = new List<Transform>();
        
        foreach (var i in pattern)
        {
            points.Add(transform.GetChild(i));
        }

        return points;
    }

    public Transform GetSpawnPoint(Vector3 avoidPosition)
    {
        int randomIdx = 0;
        float distance = 0f;
        Transform point = default;
        
        do
        {
            randomIdx = Random.Range(0, transform.childCount);
            point = transform.GetChild(randomIdx);
            distance = Vector3.Distance(avoidPosition, point.position);
        }
        while(distance <= 12f);

        
        return point;
    }
}