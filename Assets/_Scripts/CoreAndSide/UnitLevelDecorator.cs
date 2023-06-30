using System;
using UnityEngine;

public class UnitLevelDecorator : IUnitStatsProvider
{
    protected Unit _unit;
    protected IUnitStatsProvider _unitStats;
    protected UnitParameterType[] _unitParameters;

    public UnitLevelDecorator(IUnitStatsProvider unitStats, Unit unit)
    {
        _unitStats = unitStats;
        _unit = unit;
        _unitParameters = (UnitParameterType[])Enum.GetValues(typeof(UnitParameterType));
    }

    public virtual float GetStats(UnitParameterType parameterType)
    {
        float startValue;
        float perLevel;
        float level = _unit.Level - 1;
        switch (parameterType)
        {
            case UnitParameterType.Attack:
                startValue = _unitStats.GetStats(UnitParameterType.BaseAttack);
                perLevel = _unitStats.GetStats(UnitParameterType.AttackPerLevel);
                return startValue * Mathf.Pow(perLevel, level);

            case UnitParameterType.MaxHealth:
                startValue = _unitStats.GetStats(UnitParameterType.BaseHealth);
                perLevel = _unitStats.GetStats(UnitParameterType.HealthPerLevel);
                return startValue * Mathf.Pow(perLevel, level);

            case UnitParameterType.AttackSpeed:
                startValue = _unitStats.GetStats(UnitParameterType.BaseAttackSpeed);
                perLevel = _unitStats.GetStats(UnitParameterType.AttackSpeedPerLevel);
                return startValue - perLevel * level;

            case UnitParameterType.UpgradeCost:
                startValue = _unitStats.GetStats(UnitParameterType.BaseUpgradeCost);
                perLevel = _unitStats.GetStats(UnitParameterType.UpgradeCostPerLevel);
                return startValue * Mathf.Pow(perLevel, level);
        }
        return _unitStats.GetStats(parameterType);
    }
}
