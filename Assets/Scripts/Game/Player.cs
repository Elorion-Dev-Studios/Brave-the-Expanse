using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    private Vector3 _direction;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int  _maxLives = 3;
    private int _score;

    #region Overdrive_Props
    [SerializeField] private bool _overdriveActive = false;
    [SerializeField] private float _overdriveSpeed = 4.0f;
    [SerializeField] private GameObject _minorThrusters;
    [SerializeField] private float _overdriveCharge = 1.0f;
    [SerializeField] private float _overdriveDuration = 3.0f;
    //time in seconds between when overdrive is deactivated and overdrive begins recharging
    [SerializeField] private float _overdriveRechargeDelay = 1.0f;
    //time in seconds to fill overdrive charge from 0 to 100%
    [SerializeField] private float _overdriveRechargeDuration = 10.0f;
    //how frequently in second overdrive charge is increment
    [SerializeField] private float _overdriveRechargeInterval = 0.5f;
    [SerializeField] private bool _overdriveRecharging = false;
    #endregion

    #region ScreenBounds_Props
    [SerializeField] private float _screenMaxX, _screenMinX;
    [SerializeField] private float _screenMaxY, _screenMinY;
    #endregion

    #region Ammo_Props
    [SerializeField] private AmmoType _activeAmmoType = AmmoType.Laser;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private float _laserOffsetY;
    private Vector3 _laserOffsetVector;
    [SerializeField] private float _fireRate = 0.5f;
    private float _nextFire = -1f;
    private int _ammoCount;
    [SerializeField] private int _maxAmmoCount = 15;
    #endregion

    #region Powerup_Props
    private bool _tripleShotActive = false;

    [SerializeField] private float _powerupDuration = 3.0f; 
    private bool _speedBoostActive;
    [SerializeField] private float _speedBoost = 5.0f;
    private bool _thrusterActive;
    [SerializeField] private GameObject _thrusterObject;

    private bool _shieldActive;
    private int _shieldHealth = 3;
    [SerializeField] private GameObject _shieldObject;
    private SpriteRenderer _shieldRenderer;
    #endregion

    [SerializeField] private GameObject _rightEngineDamage, _leftEngineDamage;

    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private UIManager _uiManager;
    private Camera _camera;

    #region Audio_Props
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserClip;
    [SerializeField] private AudioClip _noAmmoClip;
    [SerializeField] private AudioClip _explosionClip;
    [SerializeField] private AudioClip _shieldHitClip;

    #endregion

    private SpriteRenderer _spriteRenderer;


    void Start()
    {
        transform.position = Vector3.zero;
        _direction = Vector3.zero;
        _laserOffsetVector = new Vector3(0, _laserOffsetY, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Player failed to cache reference to Spawn Manager");
        }

        _shieldObject.SetActive(false);

        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("Player failed to cache reference to UI Manager");
        }

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Player failed to cache reference to Game Manager");
        }

        _camera = GameObject.Find("MainCamera").GetComponent<Camera>();
        if (_camera == null)
        {
            Debug.LogError("Player failed to cache reference to Main Camera");
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("Player failed to cache reference to its SpriteRenderer");
        }

        _shieldRenderer = _shieldObject.GetComponent<SpriteRenderer>();
        if (_shieldRenderer == null)
        {
            Debug.LogError("Player failed to cache reference its shield SpriteRenderer");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Player failed to cache reference to its AudioSource");
        }
        else
        {
            _audioSource.clip = _laserClip;
        }

        _ammoCount = _maxAmmoCount;
    }

    void Update()
    {
        float currentSpeed = CalculateSpeed();

        CalculateMovement(currentSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && _ammoCount > 0)
        {
            FireLaser();
        } 
        else if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && _ammoCount <= 0)
        {
            _audioSource.clip = _noAmmoClip;
            _audioSource.Play();

            _uiManager.AlertNoAmmo();
        }
    }

    float CalculateSpeed()
    {
        //speedBoost powerup trumps
        if (_speedBoostActive)
        {
            return _speedBoost;
        }

        CalculateOverdrive();

        return _overdriveActive ? _overdriveSpeed : _speed;
    }

    void CalculateMovement(float speed)
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");
        transform.Translate(_direction * speed * Time.deltaTime);

        Vector3 playerPosition = transform.position;

        if (playerPosition.x > _screenMaxX)
        {
            playerPosition.x = _screenMinX;
        }
        else if (playerPosition.x < _screenMinX)
        {
            playerPosition.x = _screenMaxX;
        }

        playerPosition.y = Mathf.Clamp(transform.position.y, _screenMinY, _screenMaxY);

        transform.position = playerPosition;
    }

    void CalculateOverdrive()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) &&
            _overdriveActive == false &&
            _overdriveCharge > 0f)
        {
            _overdriveActive = true;
            _minorThrusters.SetActive(true);
        }

        else if ((Input.GetKeyDown(KeyCode.LeftShift) ||
            _overdriveCharge == 0f) &&
            _overdriveActive == true)
        {
            _overdriveActive = false;
            _minorThrusters.SetActive(false);
            StartCoroutine(OverdriveRechargeRoutine());
        }

        if (_overdriveActive && _overdriveCharge > 0f)
        {
            _overdriveCharge -= (1.0f / _overdriveDuration) * Time.deltaTime;
            _uiManager.UpdateThrusterBar(_overdriveCharge);
        }
        else if (_overdriveActive && _overdriveCharge <= 0f)
        {
            _overdriveActive = false;
            _minorThrusters.SetActive(false);
            StartCoroutine(OverdriveRechargeRoutine());
        }

    }

    void FireLaser()
    {
        _nextFire = Time.time + _fireRate;

        _ammoCount -= 1;

        switch (_activeAmmoType)
        {
            case AmmoType.Laser:
                Instantiate(_laserPrefab, (transform.position + _laserOffsetVector), Quaternion.identity);
                break;
            case AmmoType.TripleShot:
                Instantiate(_tripleShotPrefab, (transform.position), Quaternion.identity);
                break;
            case AmmoType.Bomb:
                Instantiate(_bombPrefab, (transform.position + _laserOffsetVector), Quaternion.identity);
                break;
        }

        _audioSource.clip = _laserClip;
        _audioSource.Play();

        _uiManager.UpdateAmmoText(_ammoCount.ToString());
        if (_ammoCount == 0) 
        {
            _uiManager.AlertNoAmmo();
        }
    }

    IEnumerator TripleShotRoutine(float duration)
    {
        _activeAmmoType = AmmoType.TripleShot;
        _uiManager.UpdateAmmoImg(_activeAmmoType);
        yield return new WaitForSeconds(duration);
        _activeAmmoType = AmmoType.Laser;
        _uiManager.UpdateAmmoImg(_activeAmmoType);
    }

    IEnumerator BombRoutine(float duration)
    {
        _activeAmmoType = AmmoType.Bomb;
        _uiManager.UpdateAmmoImg(_activeAmmoType);
        yield return new WaitForSeconds(duration);
        _activeAmmoType = AmmoType.Laser;
        _uiManager.UpdateAmmoImg(_activeAmmoType);
    }

    IEnumerator SpeedBoostRoutine(float duration)
    {
        _speedBoostActive = true;
        _thrusterActive = true;
        _thrusterObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        _speedBoostActive = false;
        _thrusterObject.SetActive(false);
    }

    IEnumerator OverdriveRechargeRoutine()
    {
        yield return new WaitForSeconds(_overdriveRechargeDelay);
        while (_overdriveActive == false && _overdriveCharge < 1.0f)
        {
            _overdriveCharge += (1 / _overdriveRechargeDuration) * _overdriveRechargeInterval;
            if(_overdriveCharge > 1.0f )
            {
                _overdriveCharge = 1.0f;
            }
            _uiManager.UpdateThrusterBar(_overdriveCharge);
            yield return new WaitForSeconds(_overdriveRechargeInterval);
        }
    }

    public void Damage()
    {
        if (_shieldActive)
        {
            DamageShield();
        }
        else
        {
            DamagePlayer();
        }
    }

    private void DamagePlayer()
    {
        _lives--;
        if (_lives <= 0)
        {
            _lives = 0;
        }

        _uiManager.UpdateLivesImg(_lives);

        _audioSource.clip = _explosionClip;
        _audioSource.Play();
        _camera.ActivateCameraShake();

        switch (_lives)
        {
            case 3:
                break;
            case 2:
                _rightEngineDamage.SetActive(true);
                break;
            case 1:
                _leftEngineDamage.SetActive(true);
                break;
            default:
                _spawnManager.StopSpawning();
                _uiManager.UpdateGameOver();
                _gameManager.UpdateGameOver();
                _spriteRenderer.enabled = false;
                _leftEngineDamage.SetActive(false);
                _rightEngineDamage.SetActive(false);
                _thrusterObject.SetActive(false);
                _minorThrusters.SetActive(false);
                _shieldRenderer.enabled = false;
                Destroy(this.gameObject, 2.0f);
                break;
        }

    }

    private void DamageShield()
    {
        _shieldHealth -= 1;

        //hit audio
        _audioSource.clip = _shieldHitClip;
        _audioSource.Play();

        Color currentColor = _shieldRenderer.color;

        switch (_shieldHealth)
        {
            case 3:
                break;
            case 2:
                _shieldRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, .75f);
                break;
            case 1:
                _shieldRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, .50f);
                break;
            default:
                DeactivateShield();
                break;
        }
    }

    private void DeactivateShield()
    {
        _shieldActive = false;
        _shieldObject.SetActive(false);
    }

    public void ActivateShield(float duration)
    {
        _shieldActive = true;
        _shieldHealth = 3;
        _shieldRenderer.color = Color.white;
        _shieldObject.SetActive(true);
    }

    public void ActivateTripleShot(float duration)
    {
        StartCoroutine(TripleShotRoutine(duration));
    }

    public void ActivateBomb(float duration)
    {
        StartCoroutine(BombRoutine(duration));
    }

    public void ActivateSpeedBoost(float duration)
    {
        StartCoroutine(SpeedBoostRoutine(duration));
    }

    public void ActivateAmmoRefill(float duration)
    {
        _ammoCount = _maxAmmoCount;
        _uiManager.UpdateAmmoText(_ammoCount.ToString());
    }

    public void ActivateHealthRefill(float duration)
    {
        _lives = _maxLives > _lives ? _lives + 1 : _maxLives;

        //update player engine damage
        switch (_lives)
        {
            case 3:
                _rightEngineDamage.SetActive(false);
                _leftEngineDamage.SetActive(false);
                break;
            case 2:
                _leftEngineDamage.SetActive(false);
                break;
            default:
                break;
        }

        _uiManager.UpdateLivesImg(_lives);
    }

    public void IncrementScore(int points)
    {
        _score += points;
        _uiManager.UpdateScoreText(_score.ToString());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            Destroy(other.gameObject, 0.05f);
            Damage();
        }
    }

}
