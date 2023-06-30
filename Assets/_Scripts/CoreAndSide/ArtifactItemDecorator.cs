public class ArtifactItemDecorator : IItemStatsProvider
{
    protected IItemStatsProvider _item;
    protected Artifact[] _artifacts;

    public ArtifactItemDecorator(IItemStatsProvider item, Artifact[] artifacts)
    {
        _item = item;
        _artifacts = artifacts;
    }

    public virtual float GetStats(ItemParameterType parameterType)
    {
        float baseStat = _item.GetStats(parameterType);
        float value = baseStat;
        foreach (var art in _artifacts)
        {
            value += art.GetStats(baseStat);
        }

        return value;
    }
}