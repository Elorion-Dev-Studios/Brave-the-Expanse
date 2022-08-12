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


    // Start is called before the first frame update
    void Start()
    {
        //set starting position to (0,0,0)
        transform.position = Vector3.zero;
        _direction = Vector3.zero;
        
    }

    // Update is called once per frame
    void Update()
    {
        

        //move player based on user input
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");
        transform.Translate(_direction * _speed * Time.deltaTime);

        Vector3 playerPosition = transform.position;

        //keep player between MaxX, MinX
        if (playerPosition.x > _screenMaxX)
        {
            playerPosition.x = _screenMinX;
        } 
        else if (playerPosition.x < _screenMinX)
        {
            playerPosition.x = _screenMaxX;
        }

        //keep player between MaxY, MinY
        if(playerPosition.y > _screenMaxY)
        {
            playerPosition.y = _screenMaxY;
        }
        else if (playerPosition.y < _screenMinY)
        {
            playerPosition.y = _screenMinY;
        }

        transform.position = playerPosition;
    }
}
