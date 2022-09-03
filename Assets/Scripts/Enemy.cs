using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    //min/max X position where enemy is still in view
    [SerializeField]
    private float _maxX;
    [SerializeField]
    private float _minX;

    //min/max Y position where enemy is just out of view
    [SerializeField]
    private float _maxY;
    [SerializeField]
    private float _minY;


    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < _minY)
        {
            float randomX = Random.Range(_minX, _maxX);
            transform.position = new Vector3(randomX, _maxY,0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            else
            {
                Debug.Log("Player does not exist -- Cannot damage player");
            }

            Destroy(this.gameObject);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }
}
