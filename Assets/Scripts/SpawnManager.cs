using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //reference to enemy
    [SerializeField]
    private GameObject _enemy;
    //reference to waitforseconds
    private WaitForSeconds _enemyDelay = new WaitForSeconds(5.0f);
    //enemy spawn y position
    [SerializeField]
    private float _enemySpawnY;
    //enemy spawn X max, min
    [SerializeField]
    private float _enemySpawnXMax, _enemySpawnXMin;



    // Start is called before the first frame update
    void Start()
    {
        //start spawn routine
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    //spawn game objects every 5 secs
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            //random spawn location
            Vector3 enemySpawnPos = new Vector3(Random.Range(_enemySpawnXMin, _enemySpawnXMax), _enemySpawnY, 0);
            //spawn enemy
            Instantiate(_enemy,enemySpawnPos,Quaternion.identity);
            //wait 5 sec
            yield return _enemyDelay;
        }
    }
}
