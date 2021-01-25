using UnityEngine;

public class LungeAttack : AttackPattern
{
    private enum LungeState
    {
        None,
        Preparing,
        Attacking
    }

    [SerializeField] private float _preparingTime = 2f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _attacksAmount = 3;

    private LungeState _currentState = LungeState.None;
    private Vector3 _positionToAttack = default;
    private int _currentAttack = 0;
    private bool _wallCollision = false;

    private void OnEnable()
    {
        _currentState = LungeState.Preparing;
        _currentAttack = 0;
        _timer.StartCountDown(_preparingTime, Attack);
    }

    private void Update()
    {
        switch (_currentState)
        {
            case LungeState.Preparing:
            {
                _timer.Update(Time.deltaTime);
            }
            break;

            case LungeState.Attacking:
            {
                if (!_wallCollision)
                {
                    // Move our position a step closer to the target.
                    float step =  _speed * Time.deltaTime; // calculate distance to move
                    transform.position = Vector3.MoveTowards(transform.position, _positionToAttack, step);
                }

                // Check if the position of the cube and sphere are approximately equal.
                if (_wallCollision || Vector3.Distance(transform.position, _positionToAttack) < 0.001f)
                {
                    _currentAttack++;
                    _wallCollision = false;

                    if (_currentAttack >= _attacksAmount)
                    {
                        FinishAttack();
                    }
                    else
                    {
                        _currentState = LungeState.Preparing;
                        _timer.Reset();
                    }

                }
            }
            break;
        }
    }

    private void Attack()
    {
        _currentState = LungeState.Attacking;
        _positionToAttack = _target.position;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Wall")
        {
           _wallCollision = true;
        }
    }
}