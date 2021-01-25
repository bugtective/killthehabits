using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
   [SerializeField] private List<string> _patterns = new List<string>();

    private List<int[]> _pattersIdx = default;
    private int _lastInxChosen = -1;

    private void Awake()
    {
        _pattersIdx = new List<int[]>();

        foreach (var pattern in _patterns)
        {
            string[] subs = pattern.Split(',');
            int[] indexes = new int[subs.Length];
            for (int i = 0; i < subs.Length; i++)
            {
                indexes[i] = int.Parse(subs[i]);
            }

            _pattersIdx.Add(indexes);
        }
    }

   public List<Transform> GetSpawnPoints()
   {
        List<Transform> points = new List<Transform>();

        int randomIdx = 0;
        do
        {
            randomIdx = Random.Range(0, _pattersIdx.Count);
        }
        while (randomIdx == _lastInxChosen);

        _lastInxChosen = randomIdx;

        foreach (var i in _pattersIdx[randomIdx])
        {
            points.Add(transform.GetChild(i));
        }

        return points;
   }
}