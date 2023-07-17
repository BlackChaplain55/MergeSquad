using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MergeSystem))]
[RequireComponent(typeof(AudioSource))]
public class SFXMerge : SFXComponent
{
    [SerializedDictionary(nameof(MergeActionType), nameof(SoundCUE))] public SerializedDictionary<MergeActionType, SoundCUE> Sounds;
    private MergeSystem _mergeSystem;
    private AudioSource _audioSource;

    private void Start()
    {
        _mergeSystem = GetComponent<MergeSystem>();
        _audioSource = GetComponent<AudioSource>();

        _mergeSystem.OnSlotCarryStateChanged += SlotStateChanged;
        _mergeSystem.OnItemMerged += ItemMerged;
    }

    private void SlotStateChanged(Slot slot, bool flag)
    {
        var sound = flag? Sounds[MergeActionType.OnItemPickUp] : Sounds[MergeActionType.OnItemDrop];

        _audioSource.pitch = sound.Pitch;
        _audioSource.PlayOneShot(sound.Output, sound.Volume);
    }

    private void ItemMerged(Slot slot)
    {
        Debug.Log("merged ");
        var sound = Sounds[MergeActionType.OnItemMerged];

        _audioSource.pitch = sound.Pitch;
        _audioSource.PlayOneShot(sound.Output, sound.Volume);
    }

    private void OnDestroy()
    {
        _mergeSystem.OnSlotCarryStateChanged -= SlotStateChanged;
        _mergeSystem.OnItemMerged -= ItemMerged;
    }
}

public enum MergeActionType
{
    OnItemPickUp,
    OnItemDrop,
    OnItemMerged
}