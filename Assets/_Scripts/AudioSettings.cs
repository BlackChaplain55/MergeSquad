using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _soundsVolumeParameter = "SoundsVol";
    [SerializeField] private string _musicVolumeParameter = "MusicVol";
    [SerializeField] private Slider _soundsVol;
    [SerializeField] private Slider _musicVol;
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private float _volMultiplier = 30f;
    private float _prevSoundVol = 0;
    private float _prevMusicVol = 0;

    private void Awake()
    {
        _soundsVol.onValueChanged.AddListener(HandleSoundVolChange);
        _musicVol.onValueChanged.AddListener(HandleMusicVolChange);
        _soundToggle.onValueChanged.AddListener(HandleSoundToggle);
    }

    private void HandleSoundToggle(bool soundToggle)
    {
        if (soundToggle)
        {
            _soundsVol.value = _prevSoundVol;
            _musicVol.value = _prevMusicVol;
            _soundsVol.interactable = true;
            _musicVol.interactable = true;
        } else {
            _prevSoundVol = _soundsVol.value;
            _prevMusicVol = _musicVol.value;
            _soundsVol.value = _soundsVol.minValue;
            _musicVol.value = _soundsVol.minValue;
            _soundsVol.interactable = false;
            _musicVol.interactable = false;
        };
    }

    private void HandleMusicVolChange(float vol)
    {
        _audioMixer.SetFloat(_musicVolumeParameter, Mathf.Log10(vol) * _volMultiplier);
    }

    private void HandleSoundVolChange(float vol)
    {
        _audioMixer.SetFloat(_soundsVolumeParameter, Mathf.Log10(vol)*_volMultiplier);
    }

    private 
    void Start()
    {
        _soundsVol.value = 0.8f;
        _soundsVol.value = PlayerPrefs.GetFloat(_soundsVolumeParameter, _soundsVol.value);
        _prevSoundVol = _soundsVol.value;
        _musicVol.value = 0.8f;
        _musicVol.value = PlayerPrefs.GetFloat(_musicVolumeParameter, _musicVol.value);
        _prevMusicVol = _musicVol.value;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(_soundsVolumeParameter, _soundsVol.value);
        _soundsVol.onValueChanged.RemoveAllListeners();
        _musicVol.onValueChanged.RemoveAllListeners();
        _soundToggle.onValueChanged.RemoveAllListeners();
    }
}
