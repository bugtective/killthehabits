using UnityEngine;

public class NextBomb : PoolableObject
{
    [SerializeField] float _waitTime = 1.5f;
    [SerializeField] float _explosionTime = 0.7f;

    [SerializeField] GameObject _colliderObject = default;

    private Timer _timer = new Timer();

    public void Spawn(Vector3 position)
    {
        _colliderObject.SetActive(false);
        transform.position = position;
        _timer.StartCountDown(_waitTime, Explode);
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void Explode()
    {
        _colliderObject.SetActive(true);
        _timer.StartCountDown(_explosionTime, Reset);
    }
}