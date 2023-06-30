using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ArtifactsRepository")]
public class ArtifactsRepository : ScriptableObject
{
    [SerializedDictionary(nameof(UnitType), nameof(Artifact))]
    public SerializedDictionary<UnitType, Artifact[]> UnitArtifactsData;
    [SerializedDictionary(nameof(ItemType), nameof(Artifact))]
    public SerializedDictionary<ItemType, Artifact[]> ItemArtifactsData;
    public Artifact[] this[UnitType type]
    {
        get { return UnitArtifactsData[type]; }
    }
    public Artifact[] this[ItemType type]
    {
        get { return ItemArtifactsData[type]; }
    }
    public void Init
        (
        SerializedDictionary<UnitType, Artifact[]> savedUnitArtifacts,
        SerializedDictionary<ItemType, Artifact[]> savedItemArtifacts
        )
    {
        if (savedUnitArtifacts != null && savedUnitArtifacts.Count > 0)
            UnitArtifactsData = savedUnitArtifacts;
        if (savedItemArtifacts != null && savedItemArtifacts.Count > 0)
            ItemArtifactsData = savedItemArtifacts;
    }
    private void FillArtifacts<F, T, U>(F from, T to)
        where U : System.Enum
        where F : SerializedDictionary<U, ArtifactSO[]>
        where T : Dictionary<U, Artifact[]>
    {
        foreach (var artifactByType in from)
        {
            to.Add(artifactByType.Key, new Artifact[from[artifactByType.Key].Length]);
            foreach (var artifactByParameter in from[artifactByType.Key])
            {
                new Artifact(0, artifactByParameter);
            }
        }
    }
}
