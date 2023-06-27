using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewArtifact", menuName = "Scriptables/Artifact")]
public class ArtifactSO : ScriptableObject
{
    public int MaxCount;
    public float MultiplierPerPiece;
    public ItemType ItemType;
    public UnitType UnitType;
}