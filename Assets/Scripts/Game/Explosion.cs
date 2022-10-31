using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _destroyDelay = 2.5f;

    void Start()
    {
        Destroy(this.gameObject, _destroyDelay);
    }
}
