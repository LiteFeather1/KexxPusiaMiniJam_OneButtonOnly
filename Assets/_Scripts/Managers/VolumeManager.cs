using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Volume _volume;
    [SerializeField] private Vignette _vignette;

    private void Awake()
    {
        _volume.profile.TryGet(out _vignette);
    }

    public void ActivateVignette()
    {
        _vignette.active = true;
    }

    public void DeactivateVignette()
    {
        _vignette.active = false;
    }
}
