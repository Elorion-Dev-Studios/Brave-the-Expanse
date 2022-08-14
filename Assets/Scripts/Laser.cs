using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //speed variable
    [SerializeField]
    private float _speed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move laser up at speed
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }
}
