using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioClip[] _soundtracks;
    [SerializeField] private AudioClip _comicsTrack;
    [SerializeField] private AudioClip _menuTrack;
    [SerializeField] private AudioClip _mapTrack;
    [SerializeField] private AudioSource _audioSource;

    void Awake()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }

    public void StartMusic(int gameStateIndex)
    {
        StopMusic();

        var clipsToPlay = new AudioClip[]
        {
            _comicsTrack,
            _menuTrack,
            _menuTrack,
            _mapTrack,
            _soundtracks[0]
        };

        _audioSource.clip = clipsToPlay[gameStateIndex];
        _audioSource.Play();
    }

    public void StartMusic(AudioClip clip)
    {
        if (_audioSource == null) return;
        StopMusic();
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource?.Stop();
    }
}
