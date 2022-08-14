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
    //reference to laser
    [SerializeField]
    private GameObject _laserPrefab;
    //laser position offset
    [SerializeField]
    private float _laserOffsetY;
    private Vector3 _laserOffsetVector;
    //minimum time between laser fires
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

        //if space key pressed & game run time is greater than cache next fire time
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire )
        {
            // update next fire time with current game run time plus fire rate
            _nextFire = Time.time + _fireRate;
            // create laser at player position plus offset
            Instantiate(_laserPrefab, (transform.position + _laserOffsetVector), Quaternion.identity);
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
}
