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

    private Timer _yearsTimer = new Timer();
    private Timer _willPowerTimer = new Timer();

    private int _playersAge = 18;
    private int _currentWillPower = 0;

    private static GameManager instance = null;
    private static readonly object padlock = new object();

    public bool GameFinished {get; private set;}

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

        _yearsTimer.StartCountDown(_timeForYearIncrement, ReceiveDamage);
        _willPowerTimer.StartCountDown(_timeForWillpower, SpawnWillPower);

        UpdateUI();

        GameFinished = false;

        _boss.AwakeBoss();
    }


    private void Start()
    {
        RestartGame();
    }

    private void Update()
    {
        if (GameFinished)
        {
            if (Input.GetKey(KeyCode.R))
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
        }


        UpdateUI();

        if (_playersAge >= _deathAge)
        {
            GameFinished = true;
            _boss.OnGameEnd(playerWon : false);
            Debug.Log("LOOSE :(");
        }
    }
    
    private void SpawnWillPower()
    {
        var point = _spawnPoints.GetSpawnPoint();
        var poolObject = _willpowerPool.GetObject();
        poolObject?.GetComponent<WillPowerItem>().Spawn(point.position, OnWillPowerPickUp);
        
        _willPowerTimer.Reset();
    }

    public void OnWillPowerPickUp()
    {
        _currentWillPower++;

        UpdateUI();

        if (_currentWillPower >= _willpowerNeeded)
        {
            GameFinished = true;
            _boss.OnGameEnd(playerWon : true);
            Debug.Log("WIN!!!!!!!");
        }
    }

    private void UpdateUI()
    {
        _ageText.text = $"{_playersAge}";
        _willpowerText.text = $"{_currentWillPower} / {_willpowerNeeded}";
    }
}