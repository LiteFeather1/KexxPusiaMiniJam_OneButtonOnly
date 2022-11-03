using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private int _score;

    [Header("SpawnRate")]
    private float _time;
    [SerializeField] private float _lerpTime;
    [SerializeField] private float _startTimeBetweenSpawns;
    [SerializeField] private float _endTimeBetweenSpawns;
    private float _timeBetweenSpawns;
    private float _timeElapsed;

    [Header("Handle Enemy")]
    [SerializeField] private GameObject _enemy;
    [SerializeField] private float _xPosToSpawn;
    [SerializeField] private float _yPosToSpawn;

    [Header("Handle Meteors")]
    [SerializeField] private GameObject[] _meteors;
    private bool _spawnMeteor = true;

    private float _everyMinutePassingTime;
    private int _minutesPassed;

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }
    public int Score
    {
        get => _score; 
        set
        {
            _score = value;
            UiManager.Instance.ScoreToDisplay(_score);
        }
    }


    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    private void Update()
    {
        HandleSpawn();
        HandleLerp();
        EveryMinute();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //X
        Gizmos.DrawLine(new Vector3(_xPosToSpawn, transform.position.y), new Vector3(_xPosToSpawn, _yPosToSpawn));
        Gizmos.DrawLine(new Vector3(_xPosToSpawn, transform.position.y), new Vector3(_xPosToSpawn, -_yPosToSpawn));
        Gizmos.DrawLine(new Vector3(-_xPosToSpawn, transform.position.y), new Vector3(-_xPosToSpawn, _yPosToSpawn));
        Gizmos.DrawLine(new Vector3(-_xPosToSpawn, transform.position.y), new Vector3(-_xPosToSpawn, -_yPosToSpawn));
        //Y
        Gizmos.DrawLine(new Vector3(transform.position.x, _yPosToSpawn), new Vector3(_xPosToSpawn, _yPosToSpawn));
        Gizmos.DrawLine(new Vector3(transform.position.x, _yPosToSpawn), new Vector3(-_xPosToSpawn, _yPosToSpawn));
        Gizmos.DrawLine(new Vector3(transform.position.x, -_yPosToSpawn), new Vector3(_xPosToSpawn, -_yPosToSpawn));
        Gizmos.DrawLine(new Vector3(transform.position.x, -_yPosToSpawn), new Vector3(-_xPosToSpawn, -_yPosToSpawn));
    }

    private void HandleLerp()
    {
        if (_timeElapsed <= _lerpTime)
        {
            _timeElapsed += Time.deltaTime;

            _timeBetweenSpawns = Mathf.Lerp(_startTimeBetweenSpawns, _endTimeBetweenSpawns, _timeElapsed / _lerpTime);
        }
        else
        {
            _timeBetweenSpawns = _endTimeBetweenSpawns;
        }
    }

    private void HandleSpawn()
    {
        _time += Time.deltaTime;
        if(_time >= _timeBetweenSpawns)
        {
            _time = 0;
            CreateEnemy();
            CreateMeteor();
        }
    }

    private void CreateEnemy()
    {
        GameObject newEnemy = Instantiate(_enemy);
        Enemy newEnemyEnemy = newEnemy.GetComponent<Enemy>();
        newEnemyEnemy.SetHPSpeedDamage(1 + _minutesPassed, 3 + _minutesPassed, 1 + (int)(_minutesPassed / 2));
        newEnemy.transform.position = EnemySpawnPosition(newEnemyEnemy);
    }

    private Vector2 EnemySpawnPosition(Enemy enemy)
    {
        int random = Random.Range(1, 4);
        float xPos;
        float yPos;
        switch (random)
        {
            case 1:
                 xPos = _xPosToSpawn;
                 yPos = Random.Range(-_yPosToSpawn, _yPosToSpawn);
                enemy.WhereToMove = new Vector2(-xPos, Random.Range(-_yPosToSpawn, _yPosToSpawn));
                return new Vector2(xPos, yPos);
            case 2:
                xPos = -_xPosToSpawn;
                yPos = Random.Range(-_yPosToSpawn, _yPosToSpawn);
                enemy.WhereToMove = new Vector2(-xPos, Random.Range(-_yPosToSpawn, _yPosToSpawn));
                return new Vector2(xPos, yPos);
            case 3:
                xPos = Random.Range(-_xPosToSpawn, _xPosToSpawn);
                yPos = _yPosToSpawn;
                enemy.WhereToMove = new Vector2(Random.Range(-_xPosToSpawn, _xPosToSpawn), -yPos);
                return new Vector2(xPos, yPos);
            case 4:
                xPos = Random.Range(-_xPosToSpawn, _xPosToSpawn);
                yPos = -_yPosToSpawn;
                enemy.WhereToMove = new Vector2(Random.Range(-_xPosToSpawn, _xPosToSpawn), -yPos);
                return new Vector2(xPos, yPos);
            default:
                return EnemySpawnPosition(enemy);
        }
    }

    private void HandleBestScore()
    {
        if(_score >= PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", _score);
        }
    }

    private void CreateMeteor()
    {
        _spawnMeteor = !_spawnMeteor;
        if (_spawnMeteor)
        {
            int randomNum = Random.Range(0, _meteors.Length);
            GameObject newEnemy = Instantiate(_meteors[randomNum]);
            Enemy newEnemyEnemy = newEnemy.GetComponent<Enemy>();
            newEnemyEnemy.SetHPSpeedDamage(1 + _minutesPassed, 5 + _minutesPassed, 0);
            newEnemy.transform.position = EnemySpawnPosition(newEnemyEnemy);
        }
    }

    private void EveryMinute()
    {
        if (_minutesPassed < 5)
        {
            _everyMinutePassingTime += Time.deltaTime;
            if (_everyMinutePassingTime >= 60)
            {
                _everyMinutePassingTime = 0;
                _minutesPassed++;

            }
        }
    }

    public void AddToScore(int scoreToAdd)
    {
        Score += scoreToAdd;
    }

    public void LoseGame()
    {
        HandleBestScore();
        UiManager.Instance?.DisplayDefeatScreen();
    }
}
