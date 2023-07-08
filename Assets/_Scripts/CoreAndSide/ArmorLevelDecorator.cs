using System;
using UnityEngine;

public class ArmorLevelDecorator : ItemLevelDecorator
{
    public ArmorLevelDecorator(Unit unit)
    {
        _unit = unit;
    }

    public override float GetStats(ItemParameterType parameterType)
    {
        if (_unit == null)
            return 0;
        else if (_unit.ArmorSO == null)
            return 0;
        else
        {
            EquipmentSO armor = _unit.ArmorSO;
            int armorLevel = armor.Id;
            float hpPerLevel = GameController.Game.Settings.HPPerLevel[armor.Type];
            float attackPerLevel = GameController.Game.Settings.AttackPerLevel[armor.Type];
            float attackSpeedPerLevel = GameController.Game.Settings.AttackSpeedPerLevel[armor.Type];
            float deathTimerPerLevel = GameController.Game.Settings.ItemsDeathTimerPerLevel;

            switch (parameterType)
            {
                case ItemParameterType.HP:
                    return armor.BaseHP + hpPerLevel * armorLevel;
                case ItemParameterType.Attack:
                    return armor.BaseAttack + attackPerLevel * armorLevel;
                case ItemParameterType.AttackSpeed:
                    return armor.BaseAttackSpeed * Mathf.Pow(attackSpeedPerLevel, armorLevel);
                case ItemParameterType.DeathTimer:
                    return armor.DeathTimer + deathTimerPerLevel * armorLevel;
                default:
                    return armor.GetStats(parameterType);
            }
        }
    }
}