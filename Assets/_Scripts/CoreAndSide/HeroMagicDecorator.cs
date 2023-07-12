using System;

public class HeroMagicItemDecorator : IUnitStatsProvider
{
    protected IUnitStatsProvider _unit;
    protected IItemStatsProvider _magic;

    public HeroMagicItemDecorator(
        IUnitStatsProvider unit,
        IItemStatsProvider magic)
    {
        _unit = unit;
        _magic = magic;
    }

    public virtual float GetStats(UnitParameterType parameterType)
    {
        float original = _unit.GetStats(parameterType);
        float initValue = 1;

        if (parameterType == UnitParameterType.MagicStrength)
        {
            float magicStrength = initValue + _magic.GetStats(ItemParameterType.MagicStrength);
            return _unit.GetStats(UnitParameterType.Attack) * magicStrength;
        }
        else if (parameterType == UnitParameterType.MagicRange)
        {
            float range = initValue + _magic.GetStats(ItemParameterType.MagicRange);
            return original * range;
        }

        return original;
    }
}