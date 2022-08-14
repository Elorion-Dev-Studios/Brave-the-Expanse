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


    void Start()
    {
        transform.position = Vector3.zero;
        _direction = Vector3.zero;
        
    }

    void Update()
    {
        CalculateMovement();

        //if space key pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // create laser
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
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
