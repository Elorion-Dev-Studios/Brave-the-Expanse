using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float _spawnY;
    [SerializeField]
    private float _spawnXMax, _spawnXMin;

    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _enemyContainer;
    private WaitForSeconds _enemyDelay = new WaitForSeconds(5.0f);

    //tripleshot powerup
    [SerializeField]
    private GameObject _tripleShotPowerup;
    private float _powerupSpawnDelayMin = 3.0f;
    private float _powerupSpawnDelayMax = 7.0f;


    private bool _spawnFlag = true;

    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        while (_spawnFlag)
        {
            Vector3 enemySpawnPos = new Vector3(Random.Range(_spawnXMin, _spawnXMax), _spawnY, 0);
            GameObject newEnemy = Instantiate(_enemy,enemySpawnPos,Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return _enemyDelay;
        }
    }

    IEnumerator PowerupSpawnRoutine()
    {
        //while game is running
        while (_spawnFlag)
        {
            //random x position for powerup spawn
            Vector3 powerupSpawnPos = new Vector3(Random.Range(_spawnXMin, _spawnXMax), _spawnY, 0);
            //instantiate powerup
            Instantiate(_tripleShotPowerup, powerupSpawnPos, Quaternion.identity);
            //wait random seconds (3-7)
            yield return new WaitForSeconds(Random.Range(_powerupSpawnDelayMin, _powerupSpawnDelayMax));
        }
    }


    public void StopSpawning()
    {
        _spawnFlag = false;
    }
}
