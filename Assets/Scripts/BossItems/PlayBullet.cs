using UnityEngine;

public class PlayBullet : PoolableObject
{
    [SerializeField] private float _speed = 10f;

    private Rigidbody2D _rigidbody2d = default;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector3 origin,Transform target)
    {
        transform.position = origin;
        _rigidbody2d.velocity = (target.position - transform.position).normalized * _speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ResolveCollision(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ResolveCollision(collider);
    }

    private void ResolveCollision(Collider2D collider)
    {
        if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "Projectile")
        {
            Reset();
        }
    }
}