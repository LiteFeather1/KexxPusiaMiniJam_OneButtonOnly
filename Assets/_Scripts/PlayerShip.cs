using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShip : MonoBehaviour, IDamageable
{
    [Header("Speed")]
    [SerializeField] private float _startForce;
    [SerializeField] private float _speedMagnitude;
    private float _maxSpeedMaginute;
    [SerializeField] private float _decelerationRate;
    private Vector3 _lastVelocity;

    [Header("Dash")]
    [SerializeField] private Sprite _dashing;
    [SerializeField] private Sprite _nonDashing;
    [SerializeField] private float _dashForce;
    [SerializeField] private Transform _direction1;
    [SerializeField] private RotateObject _rotateObject;
    private SpriteRenderer sr_rotateObject;
    [SerializeField] private SpriteRenderer sr_DashJet1;
    [SerializeField] private Sprite _dashingJet;
    [SerializeField] private Sprite _nonDashingJet;
    [SerializeField] private Transform _direction2;
    [SerializeField] private RotateObject _rotateObject2;
    private SpriteRenderer sr_rotateObject2;
    [SerializeField] private SpriteRenderer sr_DashJet2;
    [SerializeField] private ParticleSystem _dashParticle;
    private ParticleSystem.EmissionModule _dashEmissionModule;
    [SerializeField] private float _dashCoolDown;

    private bool _dashed;
    private bool _dashed2;
    private bool _canDash = true;
    private bool _canDash2 = true;

    [Header("Damage")]
    [SerializeField] private int _hp;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _damage;
    [SerializeField] private bool _canTakeDamage;
    [SerializeField] private ParticleSystem _damagedParticle;

    [Header("Levels")]
    [SerializeField] private AnimationCurve _levelCurve;
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _amountToKillToLevelUp;
    [SerializeField] private int _currentKillAmount;

    [SerializeField] private ScreenShake _screenShake;
    [SerializeField] private GameObject _redScreen;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    private static PlayerShip _instance;

    public int HP { get => _hp; set => _hp = value; }

    public static PlayerShip Instance { get => _instance; }


    private void Awake()
    {
        _instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        sr_rotateObject = _direction1.GetComponent<SpriteRenderer>();
        sr_rotateObject2 = _direction2.GetComponent<SpriteRenderer>();
        _dashEmissionModule = _dashParticle.emission;
    }

    private void Start()
    {
        _amountToKillToLevelUp = (int)_levelCurve.Evaluate(_currentLevel);
        AddStartingForce();
        _maxSpeedMaginute = _rb.velocity.magnitude;
        UiManager.Instance.DisplayHealthBar(_hp);
    }

    private void Update()
    {
        Dash();
        _speedMagnitude = _rb.velocity.magnitude;
        if(_rb.velocity.magnitude > 1)
            _lastVelocity = _rb.velocity;
        DashInput();
        CanTakeDamage();
        HandleSizeOfArrows();
    }

    private void FixedUpdate()
    {
        Deaccelarete();
    }

    private void OnDisable()
    {
        GameManager.Instance?.LoseGame(); 
    }

    private void AddStartingForce()
    {
        float x = Random.value < 0.5f ? Random.Range(-1, -.5f) : Random.Range(1, .5f);
        float y = Random.value < 0.5f ? Random.Range(-1, -.5f) : Random.Range(1, .5f);

        Vector2 direction = new Vector2(x, y).normalized;
        _rb.AddForce(direction * _startForce, ForceMode2D.Impulse);
    }

    private void DashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale > 0)
        {
            if (_canDash)
            {
                _dashed = true;
                StartSqueeze();
                StartCoroutine(DashCoolDown());
            }
            else if (_canDash2)
            {
                _dashed2 = true;
                StartSqueeze();
                StartCoroutine(DashCoolDown2());
            }
        }
    }

    IEnumerator DashCoolDown()
    {
        _canDash = false;
        sr_rotateObject.color = Color.gray;
        _rotateObject.StopRotation(.25f);
        yield return new WaitForSeconds(_dashCoolDown);
        _canDash = true;
        sr_rotateObject.color = Color.white;
    }

    IEnumerator DashCoolDown2()
    {
        _canDash2 = false;
        sr_rotateObject2.color = Color.gray;
        _rotateObject2.StopRotation(.25f);
        yield return new WaitForSeconds(_dashCoolDown);
        sr_rotateObject2.color = Color.white;
        _canDash2 = true;
    }
    IEnumerator Squeeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }
    }

    private void StartSqueeze()
    {
        float xAmount = _rb.velocity.normalized.x / 5;
        float yAmount = _rb.velocity.normalized.y / 5;
        StartCoroutine(Squeeze(transform.localScale.x - xAmount, transform.localScale.y - yAmount, .25f));
    }

    private void Dash()
    {
        if (_dashed)
        {
            _dashed = false;
            _rb.velocity = Vector2.zero;
            SoundManager.Instance.PlayDash();
            _rb.AddForce(_dashForce * _direction1.transform.up, ForceMode2D.Impulse);
            _screenShake.StrongShake();
        }
        else if(_dashed2)
        {
            _dashed2 = false;
            _rb.velocity = Vector2.zero;
            SoundManager.Instance.PlayDash();
            _rb.AddForce(_dashForce * _direction2.transform.up, ForceMode2D.Impulse);
            _screenShake.StrongShake();
        }
    }

    private void HandleSizeOfArrows()
    {
        //Magic number bb
        if(_canDash)
        {
            _direction1.localScale = new(1, 1.5f, 1);
            _direction2.localScale = new(1, 1, 1);
        }
        else if(_canDash2)
        {
            _direction1.localScale = new(1, 1, 1);
            _direction2.localScale = new(1, 1.5f, 1);
        }
        else
        {
            _direction1.localScale = new(1, 1, 1);
            _direction2.localScale = new(1, 1, 1);
        }
    }

    private void Deaccelarete()
    {
        if(_speedMagnitude > _maxSpeedMaginute)
        {
            _rb.velocity *= _decelerationRate;
        }
    }

    private void CanTakeDamage()
    {
        if (_speedMagnitude > _maxSpeedMaginute)
        {
            _sr.sprite = _dashing;
            sr_DashJet1.sprite = _dashingJet;
            sr_DashJet2.sprite = _dashingJet;
            _canTakeDamage = false;
            _dashEmissionModule.rateOverTime = 50f;
        }
        else
        {
            _sr.sprite = _nonDashing;
            sr_DashJet1.sprite = _nonDashingJet;
            sr_DashJet2.sprite = _nonDashingJet;
            _canTakeDamage = true;
            _dashEmissionModule.rateOverTime = 0f;
        }
    }

    public void TakeDamage(int damage)
    {
        if(_canTakeDamage)
        {
            _hp -= damage;
            UiManager.Instance.DisplayHealthBar(_hp);
            _damagedParticle.Play();
            SoundManager.Instance.PlayDamaged();
            _screenShake.WeakShake();
            StartCoroutine(RedScreen());
            if (_hp <= 0)
            {
                Die();
            }
        }
    }

    IEnumerator RedScreen()
    {
        _redScreen.SetActive(true);
        yield return new WaitForSeconds(.25f);
        _redScreen.SetActive(false);
    }

    public void Die()
    {
        Destroy(gameObject, 0);
    }

    private void BounceReflection(Collision2D collision)
    {
        var speed = _lastVelocity.magnitude;
        var direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);
        _rb.velocity = direction * Mathf.Max(speed, 3f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BounceReflection(collision);
        _screenShake.WeakShake();
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            if (!_canTakeDamage)
            {
                damageable.TakeDamage(_damage);
                AddKill();
            }
        }
    }
    #region UpgradesMethods
    public void UpgradeDamage()
    {
        _damage += 1;
    }

    public void RestoreHealth()
    {
        _maxHp++;
        _hp++;
        _hp = Mathf.Clamp(_hp, 0, _maxHp);
        UiManager.Instance.DisplayHealthBar(_hp);
    }

    public void UpgradeDashSpeed()
    {
        _dashForce += 5f;
    }

    public void UpgradeDashCoolDown()
    {
        _dashCoolDown -= 0.2f;
    }

    public void UpgradeDashDecelaration()
    {
        _decelerationRate += 0.01f;
    }

    public void UpgradeMaxSpeed()
    {
        _maxSpeedMaginute += 2f;
    }

    public void AddKill()
    {
        _currentKillAmount++;
        if(_currentKillAmount >= _amountToKillToLevelUp)
        {
            LevelUpLogic();
        }
    }

    private void LevelUpLogic()
    {
        _currentKillAmount = 0;
        _currentLevel++;
        _amountToKillToLevelUp = (int)_levelCurve.Evaluate(_currentLevel);
        _hp++;
        _hp = Mathf.Clamp(_hp, 0, _maxHp);
        UiManager.Instance.DisplayHealthBar(_hp);
        UpgradeManager.Instance.ShowUpgradeScreen();
    }
    #endregion
}
