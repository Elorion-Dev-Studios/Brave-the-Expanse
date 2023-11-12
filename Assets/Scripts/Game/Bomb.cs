using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _maxY = 7f;
    [SerializeField] private float _minY = -7f;
    [SerializeField] // 1 = up, -1 = down
    private float _direction;
    [SerializeField] // 0 = player, 1 = enemy
    private int _laserID;
    [SerializeField] private float _fuseTime = 1.5f;
    [SerializeField] private float _explosionColliderOnDelay = .5f;
    [SerializeField] private float _explosionColliderOffDelay = 1.0f;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private Collider2D _bombCollider;
    [SerializeField] private Collider2D _explosionCollider;

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.up * _direction * _speed * Time.deltaTime);

        if (transform.position.y > _maxY || transform.position.y < _minY)
        {
            if (transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        StartCoroutine(DetonationRoutine());
    }

    IEnumerator DetonationRoutine()
    {
        yield return new WaitForSeconds(_fuseTime);
        //enable explosion
        _explosion.SetActive(true);
        _speed = 0;
        //turn on explosion collider after x time, turn off after y time
        yield return new WaitForSeconds(_explosionColliderOnDelay);
        _bombCollider.enabled = false;
        _explosionCollider.enabled = true;
        yield return new WaitForSeconds(_explosionColliderOffDelay);

        if (transform.parent != null)
        {
            Destroy(this.transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }


    }
}
