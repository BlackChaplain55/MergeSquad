using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class UnitSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _dieClip;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private float _attackVol=1;
    [SerializeField] private float _dieVol=1;
    [SerializeField] private float _hitVol=1;
    private float _sfxVolume;

    private void Awake()
    {
        if (_audio == null) _audio = GetComponent<AudioSource>();
    }

    public void Init()
    {
        //_audio.volume = CombatManager.Combat.CombatSFXVolume;
        _sfxVolume = CombatManager.Combat.CombatSFXVolume;
    }

    public void PlaySound(UnitSFXType soundType)
    {
        AudioClip clip = null;
        var currentVol = 0f;
        switch (soundType)
        {
            case UnitSFXType.Attack:
                clip = _attackClip;
                currentVol = _attackVol;
                break;
            case UnitSFXType.Hit:
                clip = _hitClip;
                currentVol = _hitVol;
                break;
            case UnitSFXType.Die:
                clip = _dieClip;
                currentVol = _dieVol;
                break;
        }
        if (clip != null)
        {
            //_audio.Stop();
            _audio.PlayOneShot(clip, _sfxVolume*currentVol);
            _audio.pitch = Random.Range(0.9f,1.1f);
            //_audio.clip = clip;
            //_audio.Play();
        }
    }

    public enum UnitSFXType
    {
        Attack,
        Hit,
        Die
    }
}
