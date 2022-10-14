using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float _spawnY;
    [SerializeField]
    private float _spawnXMax, _spawnXMin;
    private bool _spawnFlag = true;

    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _enemyContainer;
    private WaitForSeconds _enemyDelay = new WaitForSeconds(5.0f);

    [SerializeField]
    private GameObject[] _powerups;
    private float _powerupSpawnDelayMin = 3.0f;
    private float _powerupSpawnDelayMax = 7.0f;


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
        while (_spawnFlag)
        {
            int powerupIndex = Random.Range(0, _powerups.Length);
            Vector3 powerupSpawnPos = new Vector3(Random.Range(_spawnXMin, _spawnXMax), _spawnY, 0);

            Instantiate(_powerups[powerupIndex], powerupSpawnPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(_powerupSpawnDelayMin, _powerupSpawnDelayMax));
        }
    }

    public void StopSpawning()
    {
        _spawnFlag = false;
    }
}
