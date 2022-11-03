using UnityEngine;

public class PlaySlime : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.Instance.PlaySlime();
    }
}
