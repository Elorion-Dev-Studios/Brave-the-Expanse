using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField] Vector3 _rotation = new Vector3(0.0f, 0.0f, 1.0f);
    [SerializeField] float _rotateSpeed = 3.0f;

    [SerializeField] GameObject _explosion;
    [SerializeField] float _asteroidDestroyDelay = 1f;

    [SerializeField] SpawnManager _spawnManager;


    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid cannot cache reference to SpawnManager script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotation * _rotateSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Instantiate(_explosion, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, _asteroidDestroyDelay);

        }
    }

}
