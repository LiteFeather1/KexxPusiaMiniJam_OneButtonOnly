using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private float _wiggleSpeed;
    [SerializeField] private float _maxRotation;

    private IEnumerator Shake()
    {
        var rotateTo = new Quaternion
        {
            eulerAngles = new Vector3(0, 0, _maxRotation)
        };


        var currentRotation = transform.rotation.z;
        var nextRotation = _maxRotation * -1f;

        var time = 0f;

        while (Mathf.Abs(nextRotation) > 0.15f)
        {
            time += Time.unscaledDeltaTime * _wiggleSpeed;
            var newRotation = Mathf.Lerp(currentRotation, nextRotation, time);
            rotateTo.eulerAngles = new Vector3(0, 0, newRotation);
            transform.rotation = rotateTo;
            if (time >= 1)
            {
                currentRotation = nextRotation;
                nextRotation = (nextRotation * 0.9f) * -1;
                time = 0;
            }

            yield return null;
        }

        rotateTo.eulerAngles = new Vector3(0, 0, 0);
        transform.rotation = rotateTo;
    }

    public void StrongShake()
    {
        _wiggleSpeed = 90;
        _maxRotation = 3;
        StartCoroutine(Shake());
    }

    public void WeakShake()
    {
        _wiggleSpeed = 150;
        _maxRotation = .5f;
        StartCoroutine(Shake());
    }
}
