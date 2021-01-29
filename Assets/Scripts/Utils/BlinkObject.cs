using UnityEngine;
using TMPro;

public class BlinkObject : MonoBehaviour
{
    public GameObject _object = default;
    public float _duration = 1f;
    private float _time = 0f;

    void Start()
    {
        if (!_object)
        {
            Debug.LogWarning($"No object to blink setup in {name}.BlinkObject. Turning the component off");
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        _time += Time.deltaTime;
        
        if (_time >= _duration)
        {
            _time = 0f;
            _object.SetActive(!_object.activeInHierarchy);
        }
    }
}
