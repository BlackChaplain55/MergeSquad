using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class GameProgress : INotifyPropertyChanged
{
    public int UnlockedLevel
    {
        get { return _unlockedLevel; }
        set
        {
            _unlockedLevel = value;
            OnPropertyChanged();
        }
    }
    public int CurrentLevel
    {
        get { return _currentLevel; }
        set
        {
            _currentLevel = value;
            OnPropertyChanged();
        }
    }
    public int Crystals
    {
        get { return _crystals; }
        set
        {
            _crystals = value;
            OnPropertyChanged();
        }
    }
    [SerializedDictionary(nameof(UnitType), nameof(Artifact))] public SerializedDictionary<UnitType, Artifact[]> UnitArtifacts;
    [SerializedDictionary(nameof(ItemType), nameof(Artifact))] public SerializedDictionary<ItemType, Artifact[]> ItemArtifacts;
    public event PropertyChangedEventHandler PropertyChanged;
    [SerializeField] private int _unlockedLevel;
    [SerializeField] private int _currentLevel;
    [SerializeField] private int _crystals;

    public GameProgress(
        ArtifactsRepository artifactsRepository,
        Dictionary<UnitType, Artifact[]> unitArtifacts,
        Dictionary<ItemType, Artifact[]> itemArtifacts,
        int crystals,
        int unlockedLevel = 1,
        int currentLevel = 1)
    {
        if (unitArtifacts.Count < artifactsRepository.UnitArtifactsData.Count)
            FillArtifactsMissedType(artifactsRepository.UnitArtifactsData, unitArtifacts);
        //if (itemArtifacts.Count < artifactsRepository.ItemArtifactsData.Count)
            //FillArtifactsMissedType(artifactsRepository.ItemArtifactsData, itemArtifacts);

        FillArtifactsMissedArtifacts(artifactsRepository.UnitArtifactsData, unitArtifacts);
        //FillArtifactsMissedArtifacts(artifactsRepository.ItemArtifactsData, ItemArtifacts);

        _crystals = crystals;
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

    private void FillArtifactsMissedType<T>(
        Dictionary<T, Artifact[]> repository,
        Dictionary<T, Artifact[]> currentArtifacts)
    {
        foreach (var k in repository)
            if (!currentArtifacts.ContainsKey(k.Key))
                currentArtifacts.Add(k.Key, repository[k.Key]);
    }

    private void FillArtifactsMissedArtifacts<T>(
        Dictionary<T, Artifact[]> repository,
        Dictionary<T, Artifact[]> currentArtifacts)
    {
        foreach (var repositoryType in repository)
        {
            foreach (var artifact in repositoryType.Value)
            {
                ArtifactSO artifactSO = artifact.BaseData;
                if (artifactSO is ArtifactUnitSO unitArtifact)
                {
                    var unitType = repositoryType.Key;
                }
                if (artifactSO is ArtifactItemSO itemArtifact)
                {
                    var itemType = repositoryType.Key;
                    Func<Artifact, bool> isSameType = e => (e.BaseData as ArtifactItemSO).ItemType == itemArtifact.ItemType;
                    if (currentArtifacts[itemType].First(isSameType) == null)
                    {
                        var currentItemArtifacts = currentArtifacts[itemType];
                        int newLength = currentItemArtifacts.Length + 1;
                        Artifact[] newItemArtifacts = new Artifact[newLength];
                        Array.Copy(currentItemArtifacts, newItemArtifacts, newLength);
                        newItemArtifacts[newLength - 1] = artifact;
                    }
                }
            }
        }
    }
}