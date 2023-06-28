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
        foreach (var art in _artifacts)
        {
            value += art.GetStats(baseStat);
        }

        return value;
    }
}