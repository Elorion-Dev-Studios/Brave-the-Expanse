using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //enemy speed
    [SerializeField]
    private float _speed = 4f;

    //min and max X position where enemy is still in view
    [SerializeField]
    private float _maxX;
    [SerializeField]
    private float _minX;

    //min and max Y position where enemy is just out of view
    [SerializeField]
    private float _maxY;
    [SerializeField]
    private float _minY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move enemy down at speed
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //if off screen,
        if (transform.position.y < _minY)
        {
            //random x value within screen bounds
            float randomX = Random.Range(_minX, _maxX);

            //reset position at top of screen with random x value
            transform.position = new Vector3(randomX, _maxY,0);
        }
    }
}
