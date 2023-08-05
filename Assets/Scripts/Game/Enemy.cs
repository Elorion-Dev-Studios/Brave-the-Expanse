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
    [SerializeField] private AudioClip _laserClip;

    //enemy laser
    [SerializeField] private GameObject _laser;
    private bool _firingLasers = false;
    private WaitForSeconds _laserDelay = new WaitForSeconds(3.0f);
    [SerializeField]
    private float _laserOffsetY = -1.0f;
    private Vector3 _laserOffsetVector;
    [SerializeField]
    private float _laserDelayMin = 2.0f;
    [SerializeField]
    private float _laserDelayMax = 6.0f;
    private IEnumerator _fireLaser;

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

        //check laser fire flag
        if (_firingLasers == false)
        {
            //enable laser fire flag
            _firingLasers = true;
            //set laser audio clip
            _audioSource.clip = _laserClip;
            //set laser position offset
            _laserOffsetVector = new Vector3(0, _laserOffsetY, 0);
            //start laser fire routine
            _fireLaser = FireLaserRoutine();
            StartCoroutine(_fireLaser);
        }
        
    }

    IEnumerator FireLaserRoutine()
    {
        //wait for delay
        yield return _laserDelay;
        while (_firingLasers)
        {
            //get new delay 
            float delayTime = Random.Range(_laserDelayMin, _laserDelayMax);
            _laserDelay = new WaitForSeconds(delayTime);

            //play laser clip
            _audioSource.Play();

            GameObject newLaser = Instantiate(_laser, transform.position + _laserOffsetVector, Quaternion.identity);            //newLaser.transform.parent = this.transform;

            yield return _laserDelay;
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
            _firingLasers = false;
            Destroy(this.gameObject.GetComponent<Collider2D>());
            StopCoroutine(_fireLaser);
            Destroy(this.gameObject,1.5f);
        }

        if (other.CompareTag("PlayerProjectile"))
        {
            Destroy(other.gameObject,0.25f);
            _player.IncrementScore(_pointValue);
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
            _firingLasers = false;
            Destroy(this.gameObject.GetComponent<Collider2D>());
            StopCoroutine(_fireLaser);
            Destroy(this.gameObject, 1.5f);
        }
    }
}
