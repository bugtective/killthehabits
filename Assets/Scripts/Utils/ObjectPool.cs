using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _object = default;
    [SerializeField] private int _amount = 10;

    private List<PoolableObject> _pool = default;
    private int _lastTaken = 0;

    private void Awake()
    {
        if (_object == null)
        {
            Debug.LogWarning("Trying to initialize a pool with no object assigned.");
            return;
        }

        _pool = new List<PoolableObject>();

        for (int i = 0; i < _amount; i++)
        {
            GameObject newObject = Instantiate(_object);
            var poolObject = newObject.GetComponent<PoolableObject>();
            poolObject.OnCreate(this.transform);
            _pool.Add(poolObject);
        }
    }

    public PoolableObject GetObject()
    {
        int attempts = 0;
        while (_pool[_lastTaken].IsTaken)
        {
            _lastTaken++;

            if (_lastTaken >= _amount)
            {
                _lastTaken = 0;
            }

            attempts++;

            if (attempts >= _amount)
            {
                Debug.LogWarning("There are no pool items available!!!");
                return null;
            }
        }

        PoolableObject poolObject = _pool[_lastTaken];
        poolObject.Take();
        return poolObject;
    }

    public void ForceAllToGoBack()
    {
        foreach (var poolObject in _pool)
        {
            poolObject.ReturnToPool();
        }
    }
}