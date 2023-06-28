using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactsRepository : MonoBehaviour
{
    public static SerializedDictionary<UnitType, ArtifactSO[]> UnitArtifactsData { get; private set; }
    public static Dictionary<UnitType, Artifact[]> UnitArtifacts;
    public static SerializedDictionary<ItemType, ArtifactSO[]> ItemArtifactsData { get; private set; }
    public static Dictionary<ItemType, Artifact[]> ItemArtifacts;
    private void Awake()
    {
        var savedUnitArtifacts = GameController.Game.GameProgress.UnitArtifacts;
        var savedItemArtifacts = GameController.Game.GameProgress.ItemArtifacts;

        if (savedUnitArtifacts != null && savedUnitArtifacts.Count > 0)
            UnitArtifacts = savedUnitArtifacts;
        else
            FillArtifacts<SerializedDictionary<UnitType, ArtifactSO[]>, Dictionary<UnitType, Artifact[]>, UnitType>(UnitArtifactsData, UnitArtifacts);
        
        if (savedItemArtifacts != null && savedItemArtifacts.Count > 0)
            ItemArtifacts = savedItemArtifacts;
        else
            FillArtifacts<SerializedDictionary<ItemType, ArtifactSO[]>, Dictionary<ItemType, Artifact[]>, ItemType>(ItemArtifactsData, ItemArtifacts);
    }
    public List<Artifact> this[ItemType type]
    {
        get { return this[type]; }
    }
    public List<Artifact> this[UnitType type]
    {
        get { return this[type]; }
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
