using System.Collections;
using UnityEngine;

public class EnemyBeta : MonoBehaviour
{
    #region Movement
    [SerializeField] private float _speed = 4f;

    [SerializeField] private float _maxX;
    [SerializeField] private float _minX;

    [SerializeField] private float _maxY;
    [SerializeField] private float _minY;
    #endregion

    #region Damage
    private int _pointValue = 10;
    private int _health = 1;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private float _deactivateOnDeathDelay;
    [SerializeField] private float _destroyOnDeathDelay;
    #endregion

    #region Visuals
    private SpriteRenderer _spriteRenderer;
    #endregion

    #region Audio
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _onDeathClip;
    [SerializeField] private AudioClip _attackClip;
    #endregion

    #region Attack
    [SerializeField] private GameObject _weapon;
    private bool _attacking = false;
    private WaitForSeconds _weaponDelay = new WaitForSeconds(3.0f);
    [SerializeField] private float _weaponOffsetY = -1.0f;
    private Vector3 _weaponOffsetVector;
    [SerializeField] private float _weaponDelayMin = 2.0f;
    [SerializeField] private float _weaponDelayMax = 6.0f;
    //reference to Coroutine
    private IEnumerator _attackRoutine;
    #endregion

    private Player _player;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Enemy failed to cache reference to Player");
        }

        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("Enemy failed to cache reference to its SpriteRenderer");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Enemy failed to cache reference to its AudioSource");
        }
    }

    void Update()
    {
        Move();

        Attack();
    }

    public void Move()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < _minY)
        {
            float randomX = Random.Range(_minX, _maxX);
            transform.position = new Vector3(randomX, _maxY, 0);
        }

    }


    public void Attack()
    {
        if (_attacking == false)
        {
            _attacking = true;
            _audioSource.clip = _attackClip;
            _weaponOffsetVector = new Vector3(0, _weaponOffsetY, 0);
            _attackRoutine = AttackRoutine();
            StartCoroutine(_attackRoutine);
        }
    }


    IEnumerator AttackRoutine()
    {
        yield return _weaponDelay;
        while (_attacking)
        {
            float delayTime = Random.Range(_weaponDelayMin, _weaponDelayMax);

            _weaponDelay = new WaitForSeconds(delayTime);

            _audioSource.Play();

            GameObject newLaser = Instantiate(_weapon, transform.position + _weaponOffsetVector, Quaternion.identity);

            yield return _weaponDelay;
        }
    }

    public void Damage()
    {
        _health -= 1;

        if (_health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        _speed = 0;
        _attacking = false;
        StopCoroutine(_attackRoutine);
        Destroy(this.gameObject.GetComponent<Collider2D>());
        _audioSource.PlayOneShot(_onDeathClip);
        _explosion.SetActive(true);

        StartCoroutine(DeathRoutine());

        _player.IncrementScore(_pointValue);
    }

    IEnumerator DeathRoutine()
    {
        Destroy(this.gameObject, _destroyOnDeathDelay);
        yield return new WaitForSeconds(_deactivateOnDeathDelay);
        _spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player.Damage();

            Damage();
        }

        if (other.CompareTag("PlayerProjectile"))
        {
            Destroy(other.gameObject, 0.25f);

            Damage();
        }
    }
}
