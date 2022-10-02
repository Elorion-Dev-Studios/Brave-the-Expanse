using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int _speed;
    [SerializeField]
    private float _minY;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < _minY)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.ActivateTripleShot();
                Destroy(this.gameObject);
            }
            else
            {
                Debug.LogError("Unable to cache Player script -- cannot trigger powerup");
            }
        }
    }
}
