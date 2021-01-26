using UnityEngine;

public class RayTV : PoolableObject
{
    public enum RayDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] float _waitTime = 1f;
    [SerializeField] float _rayTime = 1.5f;

    [SerializeField] GameObject _rayObject = default;

    private Timer _timer = new Timer();

    public void Spawn(Vector3 position, RayDirection direction)
    {
        transform.position = position;

        _rayObject.SetActive(false);
        var zRotation = 0f;
        switch (direction)
        {
            case RayDirection.Up: break;
            case RayDirection.Down: zRotation = 180f; break;
            case RayDirection.Left: zRotation = 90f; break;
            case RayDirection.Right: zRotation = -90f; break;
        }

        _rayObject.transform.localRotation = Quaternion.Euler(0f, 0f, zRotation);

        _timer.StartCountDown(_waitTime, ShootRay);
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void ShootRay()
    {
        _rayObject.SetActive(true);
        _timer.StartCountDown(_rayTime, ReturnToPool);
    }
}