using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class GameProgress : INotifyPropertyChanged
{
    public int UnlockedLevel
    {
        get { return _unlockedLevel; }
        set { _unlockedLevel = value; }
    }
    public int CurrentLevel
    {
        get { return _currentLevel; }
        set { _currentLevel = value; }
    }
    [SerializedDictionary(nameof(UnitType), nameof(Artifact))] public SerializedDictionary<UnitType, Artifact[]> UnitArtifacts;
    [SerializedDictionary(nameof(ItemType), nameof(Artifact))] public SerializedDictionary<ItemType, Artifact[]> ItemArtifacts;
    public event PropertyChangedEventHandler PropertyChanged;
    [SerializeField] private int _unlockedLevel;
    [SerializeField] private int _currentLevel;

    public GameProgress(
        int unlockedLevel = 1,
        int currentLevel = 1,
        Dictionary<UnitType, Artifact[]> unitArtifacts = null,
        Dictionary<ItemType, Artifact[]> itemArtifacts = null)
    {
        _unlockedLevel = unlockedLevel;
        _currentLevel = currentLevel;
        PropertyChanged = null;
        UnitArtifacts = new SerializedDictionary<UnitType, Artifact[]>(unitArtifacts);
        ItemArtifacts = new SerializedDictionary<ItemType, Artifact[]>(itemArtifacts);
    }

    public void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}