using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    [SerializeField]
    private float _maxX;
    [SerializeField]
    private float _minX;

    [SerializeField]
    private float _maxY;
    [SerializeField]
    private float _minY;

    //get reference to player
    private Player _player;
    private int _pointValue = 10;

    //enemy animator
    private Animator _animator;

    //enemy AudioSource
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionClip;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Enemy failed to cache reference to Player");
        }

        _animator = gameObject.GetComponent<Animator>(); 
        if(_animator == null)
        {
            Debug.LogError("Enemy failed to cache reference to its Animator");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Enemy failed to cache reference to its AudioSource");
        }

    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < _minY)
        {
            float randomX = Random.Range(_minX, _maxX);
            transform.position = new Vector3(randomX, _maxY,0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player.Damage();
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
            Destroy(this.gameObject,1.5f);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject,0.25f);
            //add score
            _player.IncrementScore(_pointValue);
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
            Destroy(this.gameObject, 1.5f);
        }
    }
}
