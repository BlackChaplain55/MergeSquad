using System;
using UnityEngine;

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
        float original = _unit.GetStats(parameterType);
        float initValue = 1;

        if (parameterType == UnitParameterType.Attack)
        {
            float weaponAttack = initValue + _weapon.GetStats(ItemParameterType.Attack);
            float armorAttack = initValue + _armor.GetStats(ItemParameterType.Attack);
            return original * weaponAttack * armorAttack;
        }
        else if (parameterType == UnitParameterType.MaxHealth)
        {
            float hp = _armor.GetStats(ItemParameterType.HP);
            float bonus = initValue + hp;
            return original * bonus;
        }
        else if (parameterType == UnitParameterType.AttackSpeed)
        {
            float bonus = _weapon.GetStats(ItemParameterType.AttackSpeed);

            return original * bonus;
        }

        return original;
    }
}
