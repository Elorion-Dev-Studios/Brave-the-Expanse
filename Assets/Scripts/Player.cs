using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private Vector3 _direction;
    [SerializeField]
    private float _screenMaxX;
    [SerializeField]
    private float _screenMinX;
    [SerializeField]
    private float _screenMaxY;
    [SerializeField]
    private float _screenMinY;
   
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _laserOffsetY;
    private Vector3 _laserOffsetVector;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextFire = -1f;
    


    void Start()
    {
        transform.position = Vector3.zero;
        _direction = Vector3.zero;
        _laserOffsetVector = new Vector3(0, _laserOffsetY, 0);
        
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
        Instantiate(_laserPrefab, (transform.position + _laserOffsetVector), Quaternion.identity);
    }
}
