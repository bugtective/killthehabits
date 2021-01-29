using UnityEngine;
using System;

public class WillPowerItem : PoolableObject
{   
    [SerializeField] private float _dissapearingTime = 5f;
    [SerializeField] private BlinkObject _blinkObject = default;
    [SerializeField] private GameObject _spriteObject = default;
    
    private Timer _timer = new Timer();

    private event Action _onPickedUp = default;

    public void Spawn(Vector3 position, Action onPickedUp)
    {
        transform.position = position;
        _onPickedUp = onPickedUp;
        _blinkObject.enabled = false;
        _spriteObject.SetActive(true);
        _timer.StartCountDown(_dissapearingTime, ReturnToPool);
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);

        if (_timer.ProgressPercentage >= 0.7 && !_blinkObject.enabled)
        {
            _blinkObject.enabled = true;
        }
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