using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _damaged, _dash, _enemyKill, _slime, _damages;
    [SerializeField] private AudioSource _effects;
    private static SoundManager _instance;

    public static SoundManager Instance { get => _instance; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void PlayEffect(AudioClip clip)
    {
        _effects.PlayOneShot(clip);
    }

    public void PlayDamaged() => PlayEffect(_damaged);
    public void PlayDash() => PlayEffect(_dash);
    public void PlayEnemyKill() => PlayEffect(_enemyKill);
    public void PlaySlime() => PlayEffect(_slime);
    public void PlayDamages() => PlayEffect(_damages);

}
