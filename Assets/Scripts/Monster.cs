using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private ObjectPool _playBulletsPool = default;

    [SerializeField] private Character _character = default;

    private float _time = 0f;


    private void Update()
    {
        _time += Time.deltaTime;

        if (_time > 2f)
        {
            _time = 0f;

            var poolObject = _playBulletsPool.GetObject();
            poolObject?.GetComponent<PlayBullet>().Shoot(_character.transform);
        }
    }
}