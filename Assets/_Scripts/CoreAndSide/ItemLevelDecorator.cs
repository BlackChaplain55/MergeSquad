using System;

public class ItemLevelDecorator : IItemStatsProvider
{
    protected int _level;
    protected IItemStatsProvider _unit;
    protected ItemParameterType[] _unitParameters;

    public ItemLevelDecorator(IItemStatsProvider unit, int level)
    {
        _unit = unit;
        _level = level;
        _unitParameters = (ItemParameterType[])Enum.GetValues(typeof(ItemParameterType));
    }

    public virtual float GetStats(ItemParameterType parameterType)
    {
        return _unit.GetStats(parameterType) * _level;
    }
}
