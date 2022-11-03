using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownTimeOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Time.timeScale = .25f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Time.timeScale = 1f;
    }
}
