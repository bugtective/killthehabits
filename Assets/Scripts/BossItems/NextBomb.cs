using UnityEngine;

public class NextBomb : PoolableObject
{
    [SerializeField] private float _waitTime = 1.5f;
    [SerializeField] private float _explosionTime = 0.7f;

    [SerializeField] private GameObject _colliderObject = default;

    [SerializeField] private Transform _spriteMaskTransform = default;

    private Timer _timer = new Timer();

    private bool _exploded = false;

    public void Spawn(Vector3 position)
    {
        _colliderObject.SetActive(false);
        transform.position = position;
        _timer.StartCountDown(_waitTime, Explode);
    }

    private void OnEnable()
    {
        _spriteMaskTransform.localScale = Vector3.one;
        _exploded = false;
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);

        if (!_exploded)
            _spriteMaskTransform.localScale = new Vector3(Mathf.Lerp(1f, 0f, _timer.ProgressPercentage), 1f, 1f);
    }

    private void Explode()
    {
        _colliderObject.SetActive(true);
        _exploded = true;
        _timer.StartCountDown(_explosionTime, ReturnToPool);
    }
}