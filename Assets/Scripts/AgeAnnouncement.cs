using UnityEngine;
using TMPro;

public class AgeAnnouncement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text = default;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private float _scaleTo = 5f;

    private Timer _timer = new Timer();
    private Color _origColor = default;

    void Awake()
    {
        gameObject.SetActive(false);
        _origColor = _text.color;
    }

    public void Show(string text)
    {
        gameObject.SetActive(true);

        transform.localScale = Vector3.one;
        _text.color = _origColor;

        _text.text = text;
        _timer.StartCountDown(_duration, Hide);
    }


    void Update()
    {
        _timer.Update(Time.deltaTime);

        var alphaValue = Mathf.Lerp(1f, 0f, _timer.ProgressPercentage);
        _text.color = new Color(_origColor.r, _origColor.g, _origColor.b, alphaValue);

        transform.localScale = Vector3.one * Mathf.Lerp(1f, _scaleTo, _timer.ProgressPercentage);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}