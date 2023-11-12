using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int _speed;
    [SerializeField]
    private float _minY;

    [SerializeField] // 0=TripleShot; 1=Speed; 2=Shield; 3=Ammo;
    private int _powerupID;

    private AudioSource _audioSource;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if( _audioSource == null)
        {
            Debug.LogError("Powerup failed to cache reference to its AudioSource");
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("Powerup failed to cache reference to its SpriteRenderer");
        }

    }


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
                    case 3:
                        player.ActivateAmmoRefill();
                        break;
                    case 4:
                        player.ActivateHealthRefill();
                        break;
                    default:
                        Debug.Log("Powerup ID is not valid");
                        break;
                }

                _audioSource.Play();
                _spriteRenderer.enabled = false;

                Destroy(this.gameObject,1.0f);
            }
            else
            {
                Debug.LogError("Unable to cache Player script -- cannot trigger powerup");
            }
        }
    }
}
