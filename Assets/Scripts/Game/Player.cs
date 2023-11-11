using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    private Vector3 _direction;
    [SerializeField] private int _lives = 3;

    //overdrive
    private bool _overdriveReady = false;
    private bool _overdriveActive = false;
    [SerializeField] private float _overdriveSpeed = 4.0f;

    [SerializeField] private float _screenMaxX, _screenMinX;
    [SerializeField] private float _screenMaxY, _screenMinY;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _laserOffsetY;
    private Vector3 _laserOffsetVector;
    [SerializeField] private float _fireRate = 0.5f;
    private float _nextFire = -1f;

    //powerups
    [SerializeField] private float _powerupDuration = 3.0f;

    private bool _tripleShotActive;
    [SerializeField] private GameObject _tripleShotPrefab;

    private bool _speedBoostActive;
    [SerializeField] private float _speedBoost = 5.0f;

    private bool _shieldActive;
    private int _shieldHealth = 3;
    [SerializeField] private GameObject _shieldObject;

    private bool _thrusterActive;
    [SerializeField] private GameObject _thrusterObject;

    [SerializeField] private GameObject _rightEngineDamage, _leftEngineDamage;


    private SpawnManager _spawnManager;
    private GameManager _gameManager;

    //ui manager
    private UIManager _uiManager;
    //score
    private int _score;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserClip;
    [SerializeField] private AudioClip _explosionClip;

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

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogError("Player failed to cache reference to its SpriteRenderer");
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
    }

    void Update()
    {
        //determine speed value
        float currentSpeed = CalculateSpeed();

        //calculate movement with current speed value
        CalculateMovement(currentSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire )
        {
            FireLaser();
        }
    }

    float CalculateSpeed()
    {
        //speedBoost powerup trumps
        if (_speedBoostActive )
        {
            return _speedBoost;
        }

        //toggle overdrive if Left-Shift pressed
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _overdriveActive = _overdriveActive ? false : true;
        }

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

    void FireLaser()
    {
        _nextFire = Time.time + _fireRate;

        if (_tripleShotActive)
        {
            Instantiate(_tripleShotPrefab, (transform.position), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, (transform.position + _laserOffsetVector), Quaternion.identity);
        }
        _audioSource.Play();
    }

    IEnumerator TripleShotRoutine()
    {
        _tripleShotActive = true;
        yield return new WaitForSeconds(_powerupDuration);
        _tripleShotActive = false;
    }

    IEnumerator SpeedBoostRoutine()
    {
        _speedBoostActive = true;
        _thrusterActive = true;
        _thrusterObject.SetActive(true);
        yield return new WaitForSeconds(_powerupDuration);
        _speedBoostActive = false;
        _thrusterObject.SetActive(false);
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
        _lives -= 1;

        _uiManager.UpdateLivesImg(_lives);

        //hit audio
        _audioSource.clip = _explosionClip;
        _audioSource.Play();
        _audioSource.clip = _laserClip;

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
                Destroy(this.gameObject, 2.0f);
                break;
        }
    }

    private void DamageShield()
    {
        _shieldHealth -= 1;

        //hit shield
        //shield hit sound
            //_audioSource.clip = _explosionClip;
            //_audioSource.Play();
        //reset to laser sound
        _audioSource.clip = _laserClip;

        switch (_shieldHealth)
        {
            case 3:
                break;
            case 2:
                //damage shield in UI - 1st hit
                break;
            case 1:
                //damage shield in UI - 2nd hit
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

    public void ActivateShield()
    {
        _shieldActive = true;
        _shieldHealth = 3;
        _shieldObject.SetActive(true);
    }

    public void ActivateTripleShot()
    {
        StartCoroutine(TripleShotRoutine());
    }

    public void ActivateSpeedBoost()
    {
        StartCoroutine(SpeedBoostRoutine());
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
