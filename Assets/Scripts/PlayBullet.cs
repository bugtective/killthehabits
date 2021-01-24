using UnityEngine;


public class PlayBullet : PoolableObject
{
    [SerializeField] private float _speed = 10f;

    private Rigidbody2D _rigidbody2d = default;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Transform target)
    {
        //_rigidbody2d.AddForce(_rigidbody2d.transform.forward * m_Speed);

        _rigidbody2d.velocity = (target.position - transform.position).normalized * _speed; 
        //clone.velocity.y=0;
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
        if (collider.gameObject.tag != "Enemy")
        {
            Reset();
        }
    }
}