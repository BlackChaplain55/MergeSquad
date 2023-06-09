using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip[] _soundtracks;
    [SerializeField] private AudioClip _menuTrack;
    [SerializeField] private AudioClip _mapTrack;
    [SerializeField] private AudioSource _audioSource;

    void Start()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        if (GameController.Game.GameState == GameController.GameStates.Menu)
        {
            _audioSource.clip = _menuTrack;
            _audioSource.loop = true;
            _audioSource.Play();
        }else if (GameController.Game.GameState == GameController.GameStates.Map)
        {
            _audioSource.clip = _mapTrack;
            _audioSource.loop = true;
            _audioSource.Play();
        }
        else if (GameController.Game.GameState == GameController.GameStates.Map)
        {
            _audioSource.clip = _soundtracks[0];
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
