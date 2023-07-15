using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Artifact
{
    public event PropertyChangedEventHandler PropertyChanged;
    public int Count
    {
        get => count;
        protected set
        {
            count = value;
            PropertyChanged?.Invoke(this, new(nameof(Count)));
        }
    }
    [field: SerializeReference] public ArtifactSO BaseData { get; protected set; }
    [SerializeField] private int count;

    public Artifact(int count, ArtifactSO baseData)
    {
        Count = count;
        BaseData = baseData;
    }

    public void SetCount(int value)
    {
        Count = value;
    }

    public virtual float GetStats(float rawStat) => rawStat * BaseData.MultiplierPerPiece * Count;

    public bool HasImpact(ItemSO item) => (BaseData as ArtifactItemSO).ItemType == item.Type;
    public bool HasImpact(UnitData unit) => (BaseData as ArtifactUnitSO).UnitType == unit.Type;

    public override string ToString() => 
        $"Artifact for {BaseData.GetKeyType}, that modifies {BaseData.GetParameterType}, we have {Count} of them";
}