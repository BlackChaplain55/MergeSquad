using UnityEngine;

public class WeaponLevelDecorator : ItemLevelDecorator
{
    public WeaponLevelDecorator(Unit unit)
    {
        _unit = unit;
    }

    public override float GetStats(ItemParameterType parameterType)
    {
        if (_unit == null)
            return 0;
        else if (_unit.WeaponSO == null)
            return 0;
        else
        {
            EquipmentSO weapon = _unit.WeaponSO;
            int weaponLevel = weapon.Id;
            float hpPerLevel = GameController.Game.Settings.HPPerLevel[weapon.Type];
            float attackPerLevel = GameController.Game.Settings.AttackPerLevel[weapon.Type];
            float attackSpeedPerLevel = GameController.Game.Settings.AttackSpeedPerLevel[weapon.Type];
            float deathTimerPerLevel = GameController.Game.Settings.ItemsDeathTimerPerLevel;

            switch (parameterType)
            {
                case ItemParameterType.HP:
                    return weapon.BaseHP + hpPerLevel * weaponLevel;
                case ItemParameterType.Attack:
                    return weapon.BaseAttack + attackPerLevel * weaponLevel;
                case ItemParameterType.AttackSpeed:
                    return weapon.BaseAttackSpeed * Mathf.Pow(attackSpeedPerLevel, weaponLevel);
                case ItemParameterType.DeathTimer:
                    return weapon.DeathTimer + deathTimerPerLevel * weaponLevel;
                default:
                    return weapon.GetStats(parameterType);
            }
        }
    }
}