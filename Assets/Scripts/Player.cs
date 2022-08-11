using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private Vector3 _direction;


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

        //up down left right
        transform.Translate(_direction * _speed * Time.deltaTime);
    }
}
