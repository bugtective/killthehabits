using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Monster _boss = default;
    [SerializeField] private Character _character = default;

    [SerializeField] private ObjectPool _willpowerPool = default;
    [SerializeField] private SpawnPoints _spawnPoints = default;

    [SerializeField] private int _initialAge = 18;
    [SerializeField] private int _deathAge = 100;
    [SerializeField] private int _yearIncrement = 1;
    [SerializeField] private float _timeForYearIncrement = 5f;
    [SerializeField] private float _speedReduction = 1f;

    [SerializeField] private int _willpowerNeeded = 5;
    [SerializeField] private float _timeForWillpower = 5f;

    [SerializeField] private int _playButtonDamage = 5;
    [SerializeField] private int _monsterDamage = 5;
    [SerializeField] private int _nextEpisodeDamage = 5;
    [SerializeField] private int _rayDamage = 5;

    public int PlayButtonDamage => _playButtonDamage;
    public int MonsterDamage => _monsterDamage;
    public int NextEpisodeDamage => _nextEpisodeDamage;
    public int RayDamage => _rayDamage;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _ageText = default;
    [SerializeField] private TextMeshProUGUI _willpowerText = default;
    [SerializeField] private AgeAnnouncement _ageAnnouncement = default;

    [SerializeField] private GameObject _winScreen = default;
    [SerializeField] private GameObject _loseScreen = default;
    [SerializeField] private GameObject _hud = default;
    [SerializeField] private GameObject _mainMenu = default;
    [SerializeField] private TextMeshProUGUI _winMessage = default;

    [Header("Sound")]
    [SerializeField] private AudioSource _willpowerAudioSource = default;
    [SerializeField] private AudioSource _musicAudioSource = default;

    private Timer _yearsTimer = new Timer();
    private Timer _willPowerTimer = new Timer();

    private int _playersAge = 18;
    private int _currentWillPower = 0;

    private static GameManager instance = null;
    private static readonly object padlock = new object();

    public bool GameFinished {get; private set;}

    private bool _mainMenuOn = true;

    public static GameManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                }
                return instance;
            }
        }
    }

    private void RestartGame()
    {
        _playersAge = _initialAge;
        _currentWillPower = 0;
        _mainMenuOn = false;

        _yearsTimer.StartCountDown(_timeForYearIncrement, ReceiveDamage);
        _willPowerTimer.StartCountDown(_timeForWillpower, SpawnWillPower);

        _mainMenu.SetActive(false);
        _winScreen.SetActive(false);
        _loseScreen.SetActive(false);

        _hud.SetActive(true);

        UpdateUI();

        GameFinished = false;

        _character.ChangeLooks(_playersAge);
        _boss.AwakeBoss();

        _musicAudioSource.Play();
    }

    private void GameOver(bool playerWon)
    {
        GameFinished = true;
        _boss.OnGameEnd(playerWon);

        _winScreen.SetActive(playerWon);
        _loseScreen.SetActive(!playerWon);

        if (playerWon)
        {
            _winMessage.text = $"You beat your binging habit at {_playersAge}!";
        }
        
        _hud.SetActive(false);
        _ageAnnouncement.gameObject.SetActive(false);

        _musicAudioSource.Stop();
    }


    private void Start()
    {
        _mainMenu.SetActive(true);
        _mainMenuOn = true;
    }

    private void Update()
    {
        if (_mainMenuOn)
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                RestartGame();
            }
            return;
        }
        else if (GameFinished)
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                RestartGame();
            }

            return;
        }
        
        // TODO: Maybe wait until the boss finish it´s appearing animation to start the countdown
        _yearsTimer.Update(Time.deltaTime);
        _willPowerTimer.Update(Time.deltaTime);
    }

    private void ReceiveDamage()
    {
        ReceiveDamage(_yearIncrement);
        _yearsTimer.Reset();
    }

    public void ReceiveDamage(int amount)
    {
        var prevAge = _playersAge;
        _playersAge += amount;

        if (prevAge < 30 && _playersAge >= 30)
        {
            _ageAnnouncement.Show("You're in your:\nThirties!");
            _character.ReduceSpeed(_speedReduction);
        }
        else if (prevAge < 40 && _playersAge >= 40)
        {
            _ageAnnouncement.Show("You're in your:\nForties!");
            _character.ReduceSpeed(_speedReduction / 2f);
            _character.ChangeLooks(_playersAge);
        }
        else if (prevAge < 50 && _playersAge >= 50)
        {
            _ageAnnouncement.Show("You're in your:\nFifties!");
            _character.ReduceSpeed(_speedReduction / 2f);
        }
        else if (prevAge < 60 && _playersAge >= 60)
        {
            _ageAnnouncement.Show("You're in your:\nSixties");
            _character.ReduceSpeed(_speedReduction);
            _character.ChangeLooks(_playersAge);
        }


        UpdateUI();

        if (_playersAge >= _deathAge)
        {
            GameOver(playerWon : false);
        }
    }
    
    private void SpawnWillPower()
    {
        var point = _spawnPoints.GetSpawnPoint(_character.transform.position);
        var poolObject = _willpowerPool.GetObject();
        poolObject?.GetComponent<WillPowerItem>().Spawn(point.position, OnWillPowerPickUp);
        
        _willPowerTimer.Reset();
    }

    public void OnWillPowerPickUp()
    {
        _willpowerAudioSource.Play();
        _currentWillPower++;

        UpdateUI();

        if (_currentWillPower >= _willpowerNeeded)
        {
            GameOver(playerWon : true);
        }
    }

    private void UpdateUI()
    {
        _ageText.text = $"{_playersAge}";
        _willpowerText.text = $"{_currentWillPower} / {_willpowerNeeded}";
    }
}