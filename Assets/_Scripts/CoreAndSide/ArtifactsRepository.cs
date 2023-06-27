using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactsRepository : MonoBehaviour
{
    public static SerializedDictionary<UnitType, SerializedDictionary<UnitParameterType, ArtifactSO>> UnitArtifactsData { get; private set; }
    public static Dictionary<UnitType, Dictionary<UnitParameterType, Artifact>> UnitArtifacts;
    public static SerializedDictionary<ItemType, SerializedDictionary<ItemParameterType, ArtifactSO>> ItemArtifactsData { get; private set; }
    public static Dictionary<ItemType, Dictionary<ItemParameterType, Artifact>> ItemArtifacts;
    private void Awake()
    {
        var savedUnitArtifacts = GameController.Game.GameProgress.UnitArtifacts;
        var savedItemArtifacts = GameController.Game.GameProgress.ItemArtifacts;

        if (savedUnitArtifacts != null && savedUnitArtifacts.Count > 0)
            UnitArtifacts = savedUnitArtifacts;
        else
            FillArtifacts<SerializedDictionary<UnitType, SerializedDictionary<UnitParameterType, ArtifactSO>>, Dictionary<UnitType, Dictionary<UnitParameterType, Artifact>>, UnitType, UnitParameterType>( UnitArtifactsData, UnitArtifacts );
        
        if (savedItemArtifacts != null && savedItemArtifacts.Count > 0)
            ItemArtifacts = savedItemArtifacts;
        else
            FillArtifacts<SerializedDictionary<ItemType, SerializedDictionary<ItemParameterType, ArtifactSO>>, Dictionary<ItemType, Dictionary<ItemParameterType, Artifact>>, ItemType, ItemParameterType>(ItemArtifactsData, ItemArtifacts);
    }
    public List<Artifact> this[ItemType type]
    {
        get { return this[type]; }
    }
    public List<Artifact> this[UnitType type]
    {
        get { return this[type]; }
    }
    private void FillArtifacts<F, T, U, I>(F from, T to)
        where U : System.Enum
        where I : System.Enum
        where F : SerializedDictionary<U, SerializedDictionary<I, ArtifactSO>>
        where T : Dictionary<U, Dictionary<I, Artifact>>
    {
        foreach (var byType in from)
        {
            var newDict = new Dictionary<I, Artifact>();
            to.Add(byType.Key, newDict);
            foreach (var art in from[byType.Key])
            {
                {
                    to[byType.Key].Add(art.Key, new Artifact(0, art.Value));
                }
            }
        }
    }
}
