using System;
using UnityEngine;

public class UnitLevelDecorator : IUnitStatsProvider
{
    protected int _level;
    protected IUnitStatsProvider _unit;
    protected UnitParameterType[] _unitParameters;

    public UnitLevelDecorator(IUnitStatsProvider unit, int level)
    {
        _unit = unit;
        _level = level;
        _unitParameters = (UnitParameterType[])Enum.GetValues(typeof(UnitParameterType));
    }

    public virtual float GetStats(UnitParameterType parameterType)
    {
        if (parameterType == UnitParameterType.Attack)
        {
            float origStats = _unit.GetStats(UnitParameterType.BaseAttack);
            return origStats + origStats * Mathf.Pow(1.25f, _level);
        }
        else if (parameterType == UnitParameterType.MaxHealth)
        {
            float origStats = _unit.GetStats(UnitParameterType.BaseHealth);
            return origStats + origStats * Mathf.Pow(1.25f, _level);
        }
        return _unit.GetStats(parameterType);
    }
}
