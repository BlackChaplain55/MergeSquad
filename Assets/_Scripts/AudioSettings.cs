using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
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
        string volumeParameter = VolumeParameters.MusicVolumeParameter;
        PlayerPrefs.SetInt(volumeParameter, Convert.ToInt32(vol * 1000));
        PlayerPrefs.Save();
        _audioMixer.SetFloat(volumeParameter, Mathf.Log10(vol) * _volMultiplier);
    }

    private void HandleSoundVolChange(float vol)
    {
        string volumeParameter = VolumeParameters.SoundsVolumeParameter;
        PlayerPrefs.SetInt(volumeParameter, Convert.ToInt32(vol * 1000));
        PlayerPrefs.Save();
        _audioMixer.SetFloat(volumeParameter, Mathf.Log10(vol) * _volMultiplier);
    }

    private void Start()
    {
        string soundsParameter = VolumeParameters.SoundsVolumeParameter;
        string musicParameter = VolumeParameters.MusicVolumeParameter;
        _soundsVol.value = PlayerPrefs.GetInt(soundsParameter, 1000) * 0.001f;
        _prevSoundVol = _soundsVol.value;
        _musicVol.value = PlayerPrefs.GetInt(musicParameter, 1000) * 0.001f;
        _prevMusicVol = _musicVol.value;
    }

    private void OnDisable()
    {
        _soundsVol.onValueChanged.RemoveAllListeners();
        _musicVol.onValueChanged.RemoveAllListeners();
        _soundToggle?.onValueChanged.RemoveAllListeners();
    }
}
