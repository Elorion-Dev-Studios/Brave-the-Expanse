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

    private bool _tripleShotActive;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _tripleShotDuration = 3.0f;

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
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire )
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");
        transform.Translate(_direction * _speed * Time.deltaTime);

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
        yield return new WaitForSeconds(_tripleShotDuration);
        _tripleShotActive = false; 
    }

    public void Damage()
    {
        _lives -= 1;

        if (_lives < 1)
        {
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        StartCoroutine(TripleShotRoutine());
    }

}
