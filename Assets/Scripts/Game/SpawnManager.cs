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

    //powerup with weight values
    //  0-19 = 0
    // 20-39 = 1
    // 40-59 = 2
    // 



    void Start()
    {
        _spawnFlag = false;
    }

    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);
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
        yield return new WaitForSeconds(Random.Range(_powerupSpawnDelayMin, _powerupSpawnDelayMax));
        while (_spawnFlag)
        {
            int powerupIndex;
            int weightedRandom = Random.Range(0, 100);
            
            //TripleShot - common
            if (weightedRandom < 20) { powerupIndex = 0; }
            //Speed - common
            else if (weightedRandom < 40) { powerupIndex = 1; }
            //Shield - common
            else if (weightedRandom < 60) { powerupIndex = 2; }
            //Ammo - common
            else if (weightedRandom < 80) { powerupIndex = 3; }
            //health - uncommon
            else if (weightedRandom < 95) { powerupIndex = 4; }
            //bomb - rare
            else { powerupIndex = 5; }


            Vector3 powerupSpawnPos = new Vector3(Random.Range(_spawnXMin, _spawnXMax), _spawnY, 0);

            Instantiate(_powerups[powerupIndex], powerupSpawnPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(_powerupSpawnDelayMin, _powerupSpawnDelayMax));
        }
    }

    public void StartSpawning()
    {
        _spawnFlag = true;
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());

    }

    public void StopSpawning()
    {
        _spawnFlag = false;
    }
}
