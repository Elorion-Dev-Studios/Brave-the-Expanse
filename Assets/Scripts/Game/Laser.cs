using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _maxY = 7f;
    [SerializeField]
    private float _minY = -7f;
    [SerializeField] // 1 = up, -1 = down
    private float _direction;
    [SerializeField] // 0 = player, 1 = enemy
    private int _laserID;


    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.up * _direction * _speed * Time.deltaTime);

        if (transform.position.y > _maxY || transform.position.y < _minY)
        {
            if (transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
