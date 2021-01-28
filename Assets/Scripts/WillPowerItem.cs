using UnityEngine;
using System;

public class WillPowerItem : PoolableObject
{   
    [SerializeField] private float _dissapearingTime = 5f;
    
    private Timer _timer = new Timer();

    private event Action _onPickedUp = default;

    public void Spawn(Vector3 position, Action onPickedUp)
    {
        transform.position = position;
        _onPickedUp = onPickedUp;
        _timer.StartCountDown(_dissapearingTime, ReturnToPool);
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _onPickedUp?.Invoke();
            ReturnToPool();
        }
    }

}