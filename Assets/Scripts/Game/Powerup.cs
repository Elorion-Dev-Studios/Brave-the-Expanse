using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int _speed;
    [SerializeField]
    private float _minY;

    [SerializeField] // 0=TripleShot; 1=Speed; 2=Shield;
    private int _powerupID;


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
                switch(_powerupID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                    default:
                        Debug.Log("Powerup ID is not valid");
                        break;
                }

                Destroy(this.gameObject);
            }
            else
            {
                Debug.LogError("Unable to cache Player script -- cannot trigger powerup");
            }
        }
    }
}
