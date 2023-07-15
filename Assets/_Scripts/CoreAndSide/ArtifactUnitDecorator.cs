using UnityEngine;
using System.Linq;

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

        if (_unit is Hero)
            foreach (Artifact artifact in _artifacts)
                Debug.Log(artifact.ToString());

        if (!IsArtifactAffectThisStat(parameterType))
            return baseStat;

        float value = baseStat;
        foreach (var art in _artifacts)
        {
            value += art.GetStats(baseStat);
        }

        return value;
    }

    private bool IsArtifactAffectThisStat(UnitParameterType parameterType)
    {
        foreach (var artifact in _artifacts)
            if (artifact.BaseData is ArtifactUnitSO unitArtifact)
                if (unitArtifact.UnitParameterType == parameterType)
                    return true;

        return false;
    }
}