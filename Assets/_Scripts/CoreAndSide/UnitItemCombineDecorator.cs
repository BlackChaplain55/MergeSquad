using System;

public class CombineUnitItemDecorator : IUnitStatsProvider
{
    protected IUnitStatsProvider _unit;
    protected IItemStatsProvider _weapon;
    protected IItemStatsProvider _armor;
    protected UnitParameterType[] _unitParameters;

    public CombineUnitItemDecorator(
        IUnitStatsProvider unit,
        IItemStatsProvider weapon,
        IItemStatsProvider armor)
    {
        _unit = unit;
        _weapon = weapon;
        _armor = armor;
        _unitParameters = (UnitParameterType[])Enum.GetValues(typeof(UnitParameterType));
    }

    public virtual float GetStats(UnitParameterType parameterType)
    {
        if (parameterType == UnitParameterType.Attack)
            return _unit.GetStats(parameterType) + _weapon.GetStats(ItemParameterType.Power);
        else if (parameterType == UnitParameterType.MaxHealth)
            return _unit.GetStats(parameterType) + _armor.GetStats(ItemParameterType.Power);

        return _unit.GetStats(parameterType);
    }
}
