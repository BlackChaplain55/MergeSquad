using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXGlobalEvents : MonoBehaviour
{
    [SerializedDictionary(nameof(GlobalEventType), nameof(SoundCUE))] public SerializedDictionary<GlobalEventType, SoundCUE> Sounds;
    private AudioSource _audioSource;

    private void Start()
    {
        var subscribers = EventBus.OnFinalBossDeath.GetInvocationList();
        _audioSource = GetComponent<AudioSource>();

        foreach (var sub in subscribers)
        {
            Debug.Log($"{nameof(SFXGlobalEvents)} : {sub.Method.Name},");
        }
        
        //EventBus.OnFinalBossDeath += () => PlaySFX(GlobalEventType.Victory);
        //EventBus.OnHeroDeath += () => PlaySFX(GlobalEventType.Defeat);
        //EventBus.OnBossDeath += () => PlaySFX(GlobalEventType.OnCrystalGathered);
        //EventBus.OnUnitDeath += unit => { if (unit.isEnemy) PlaySFX(GlobalEventType.OnUnitDeath); };
    }

    private void PlaySFX(GlobalEventType eventType)
    {
        var sound = Sounds[eventType];

        if (sound == null)
            return;

        _audioSource.PlayOneShot(sound.Output, sound.Volume);
        //AudioSource.PlayClipAtPoint(sound.Output, Vector3.zero, sound.Volume);
    }

    private void OnDestroy()
    {
        //EventBus.OnFinalBossDeath -= () => PlaySFX(GlobalEventType.Victory);
        //EventBus.OnHeroDeath -= () => PlaySFX(GlobalEventType.Defeat);
        //EventBus.OnBossDeath -= () => PlaySFX(GlobalEventType.OnCrystalGathered);
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