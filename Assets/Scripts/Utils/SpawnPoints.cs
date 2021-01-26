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
}