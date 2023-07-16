using UnityEngine;
using System.Linq;
using Unity.Burst.Intrinsics;

public class ArtifactUnitDecorator : IUnitStatsProvider
{
    protected IUnitStatsProvider _unit;
    protected Artifact[] _artifacts;

    public ArtifactUnitDecorator(IUnitStatsProvider unit, Artifact[] artifacts)
    {
        _unit = unit;
        _artifacts = artifacts;
    }

    public virtual float GetStats(UnitParameterType parameterType)
    {
        float baseStat = _unit.GetStats(parameterType);

        float value = baseStat;

        foreach (var artifact in _artifacts)
            if (artifact.BaseData is ArtifactUnitSO unitArtifact)
                if (unitArtifact.UnitParameterType == parameterType)
                {
                    value = baseStat;
                    value += artifact.GetStats(baseStat);
                }

        return value;
    }

    private bool IsArtifactAffectThisStat(UnitParameterType parameterType)
    {

                    return true;

        return false;
    }
}