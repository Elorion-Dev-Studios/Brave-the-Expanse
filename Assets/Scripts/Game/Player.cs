using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private Vector3 _direction;
    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private float _screenMaxX, _screenMinX;
    [SerializeField]
    private float _screenMaxY, _screenMinY;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _laserOffsetY;
    private Vector3 _laserOffsetVector;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextFire = -1f;


    [SerializeField]
    private float _powerupDuration = 3.0f;

    private bool _tripleShotActive;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    private bool _speedBoostActive;
    [SerializeField]
    private float _speedBoost = 5.0f;

    private bool _shieldActive;
    [SerializeField]
    private GameObject _shieldObject;

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

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
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
            //assign laser clip
            _audioSource.clip = _laserClip;
        }
    }

    void Update()
    {
        if (_speedBoostActive)
        {
            CalculateMovement(_speedBoost);
        }
        else
        {
            CalculateMovement(_speed);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire )
        {
            FireLaser();
        }
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

        //play laser clip
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
            DeactivateShield();
            return;
        }

        _lives -= 1;

        _uiManager.UpdateLivesImg(_lives);

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
                _audioSource.clip = _explosionClip;
                _spriteRenderer.enabled = false;
                _leftEngineDamage.SetActive(false);
                _rightEngineDamage.SetActive(false);
                _audioSource.Play();
                Destroy(this.gameObject,2.0f);
                break;
        }

/*        if (_lives < 1)
        {
            _spawnManager.StopSpawning();
            _uiManager.UpdateGameOver();
            _gameManager.UpdateGameOver();
            Destroy(this.gameObject);
        }*/

        
    }

    private void DeactivateShield()
    {
        _shieldActive = false;
        _shieldObject.SetActive(false);
    }


    public void ActivateTripleShot()
    {
        StartCoroutine(TripleShotRoutine());
    }

    public void ActivateSpeedBoost()
    {
        StartCoroutine(SpeedBoostRoutine());
    }

    public void ActivateShield()
    {
        _shieldActive = true;
        _shieldObject.SetActive(true);
    }

    //public method to update score
    public void IncrementScore(int points)
    {
        //increment score by 10
        _score += points;
        //update ui manager - score text
        _uiManager.UpdateScoreText(_score.ToString());
    }

}
