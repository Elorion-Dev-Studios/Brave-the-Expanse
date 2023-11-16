using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _shakeAmplitude;
    [SerializeField] private bool _shakeInProgress = false;


    private IEnumerator ShakeRoutine()
    {
        _shakeInProgress = true;
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.05f);

            if (i == 5)
            {
                this.transform.localPosition = _startPosition;
            }
            else if (i % 2 == 0)
            {
                this.transform.localPosition = _startPosition - _shakeAmplitude;
            }
            else
            {
                this.transform.localPosition = _startPosition + _shakeAmplitude;
            }

        }
        _shakeInProgress = false;
    }

    public void ActivateCameraShake()
    {
        if (!_shakeInProgress)
        {
            StartCoroutine(ShakeRoutine());
        }

    }
}
