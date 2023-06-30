public abstract class ItemLevelDecorator : IItemStatsProvider
{
    protected Unit _unit;
    public virtual float GetStats(ItemParameterType parameterType) => 0;
}