using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    private float _storedRotateSpeed;

    private void Start()
    {
        _storedRotateSpeed = _rotateSpeed;
    }

    private void Update()
    {
        RotateObjectLogic();
    }

    private void RotateObjectLogic()
    {
        transform.Rotate(new Vector3(0f, 0f, _rotateSpeed * Time.unscaledDeltaTime));
    }

    public void StopRotation(float seconds)
    {
        StartCoroutine(CoStopRotation(seconds));
    }

    IEnumerator CoStopRotation(float seconds)
    {
        _rotateSpeed = 0;
        yield return new WaitForSeconds(seconds);
        _rotateSpeed = _storedRotateSpeed;
    }
}
