using System;
using UnityEngine;

[Serializable]
public class Artifact
{
    [field: SerializeField] public int Count { get; protected set; }
    [field: SerializeReference] public ArtifactSO BaseData { get; protected set; }

    public Artifact(int count, ArtifactSO baseData)
    {
        Count = count;
        BaseData = baseData;
    }

    public virtual float GetStats(float rawStat) => rawStat * BaseData.MultiplierPerPiece * Count;

    public bool HasImpact(ItemSO item) => (BaseData as ArtifactItemSO).ItemType == item.Type;
    public bool HasImpact(UnitData unit) => (BaseData as ArtifactUnitSO).UnitType == unit.Type;
}