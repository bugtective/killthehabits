using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    private SpriteRenderer _spriteRenderer = default;
    private Rigidbody2D _rigidBody2D = default;

    private Vector3 _moveDir = default;

    private GameManager _gameManager = default;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _gameManager = GameManager.Instance;
    }

    void Update()
    {
        var moveX = 0f;
        var moveY = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f;
        }
        
        if (moveX != 0f)
        {
            _spriteRenderer.flipX = moveX < 0f;
        }

        _moveDir = new Vector3(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        _rigidBody2D.MovePosition(transform.position + _moveDir * _speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collsion)
    {
        if (collsion.collider.gameObject.tag == "Projectile")
        {
           _gameManager.ReceiveDamage(_gameManager.PlayButtonDamage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            _gameManager.ReceiveDamage(_gameManager.MonsterDamage);
        }
        else  if (collider.gameObject.tag == "Bomb")
        {
            _gameManager.ReceiveDamage(_gameManager.NextEpisodeDamage);
        }
        else  if (collider.gameObject.tag == "Ray")
        {
            _gameManager.ReceiveDamage(_gameManager.RayDamage);
        }
    }

    public void ReduceSpeed(float amount)
    {
        _speed -= amount;
    }
}