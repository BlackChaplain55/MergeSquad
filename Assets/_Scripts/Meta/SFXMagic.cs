using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXMagic : SFXComponent
{
    [SerializedDictionary(nameof(SpellType), nameof(SoundCUE))] public SerializedDictionary<SpellType, SoundCUE> Sounds;
    [SerializeField] private MagicSystem magicSystem;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        magicSystem.OnMeteorLaunched += OnMeteorLaunched;
    }

    private void OnMeteorLaunched()
    {
        var sound = Sounds[SpellType.Meteor];

        _audioSource.pitch = sound.Pitch;
        _audioSource.PlayOneShot(sound.Output, sound.Volume);
    }

    private void OnDestroy()
    {
        magicSystem.OnMeteorLaunched -= OnMeteorLaunched;
    }
}

public enum SpellType
{
    Meteor
}