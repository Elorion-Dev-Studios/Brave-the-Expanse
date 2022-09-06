using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private float _enemySpawnY;
    [SerializeField]
    private float _enemySpawnXMax, _enemySpawnXMin;
    [SerializeField]
    private GameObject _enemyContainer;
    private WaitForSeconds _enemyDelay = new WaitForSeconds(5.0f);

    private bool _spawnFlag = true;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (_spawnFlag)
        {
            Vector3 enemySpawnPos = new Vector3(Random.Range(_enemySpawnXMin, _enemySpawnXMax), _enemySpawnY, 0);
            GameObject newEnemy = Instantiate(_enemy,enemySpawnPos,Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return _enemyDelay;
        }
    }

    public void StopSpawning()
    {
        _spawnFlag = false;
    }
}
