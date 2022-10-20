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

    //reference to shield
    [SerializeField]
    private GameObject _shieldObject;
    //shield active
    [SerializeField]
    private bool _shieldActive;


    private SpawnManager _spawnManager;


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

        //ensure shield does not display at start
        _shieldObject.SetActive(false);
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
        yield return new WaitForSeconds(_powerupDuration);
        _speedBoostActive = false;
    }

    public void Damage()
    {
        //if shield active
        if (_shieldActive)
        {
            //turn off shield
            DeactivateShield();
            //do not damage player
            return;
        }

        _lives -= 1;

        if (_lives < 1)
        {
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }

    private void DeactivateShield()
    {
        //shield active to false
        _shieldActive = false;
        //turn off shield display
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

    //public method to activate shield
    public void ActivateShield()
    {
        //set shieldactive to true
        _shieldActive = true;
        //display shield
        _shieldObject.SetActive(true);
    }

}
