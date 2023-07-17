using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXGlobalEvents : MonoBehaviour
{
    [SerializedDictionary(nameof(GlobalEventType), nameof(SoundCUE))] public SerializedDictionary<GlobalEventType, SoundCUE> Sounds;

    private void Start()
    {
        EventBus.OnHeroDeath += () => PlaySFX(GlobalEventType.Victory);
        EventBus.OnHeroDeath += () => PlaySFX(GlobalEventType.Defeat);
        EventBus.OnHeroDeath += () => PlaySFX(GlobalEventType.OnCrystalGathered);
    }

    private void PlaySFX(GlobalEventType eventType)
    {
        var sound = Sounds[eventType];

        AudioSource.PlayClipAtPoint(sound.Output, Vector3.zero, sound.Volume);
    }
}

public enum GlobalEventType
{
    Victory,
    Defeat,
    OnBossKill,
    OnUnitDeath,
    OnCrystalGathered
}