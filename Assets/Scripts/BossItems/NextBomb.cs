using UnityEngine;

public class NextBomb : PoolableObject
{
    [SerializeField] private float _waitTime = 1.5f;
    [SerializeField] private float _explosionTime = 0.7f;

    [SerializeField] private GameObject _colliderObject = default;
    [SerializeField] private GameObject _explosionObject = default;
    [SerializeField] private GameObject _visualsObject = default;

    [SerializeField] private Transform _spriteMaskTransform = default;

    private Timer _timer = new Timer();

    private bool _waitingToAppear = false;
    private bool _exploded = false;

    public void Spawn(Vector3 position, float delay = 0f)
    {
        _colliderObject.SetActive(false);
        _explosionObject.SetActive(false);

        transform.position = position;

        if (delay > 0f)
        {
            _waitingToAppear = true;
            _visualsObject.SetActive(false);
            _timer.StartCountDown(delay, Appear);
        }
        else
        {
            Appear();
        }
    }

    private void Appear()
    {
        _waitingToAppear = false;
        _visualsObject.SetActive(true);
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

        if (!_exploded && !_waitingToAppear)
            _spriteMaskTransform.localScale = new Vector3(Mathf.Lerp(1f, 0f, _timer.ProgressPercentage), 1f, 1f);
    }

    private void Explode()
    {
        _colliderObject.SetActive(true);
        _explosionObject.SetActive(true);
        _visualsObject.SetActive(false);
        _exploded = true;
        _timer.StartCountDown(_explosionTime, ReturnToPool);
    }
}