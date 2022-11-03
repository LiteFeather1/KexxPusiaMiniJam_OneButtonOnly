using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezeOnColl : MonoBehaviour
{
    IEnumerator Squeeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
         StartCoroutine(Squeeze(0.9f, 0.9f, 0.25f));
    }
}
