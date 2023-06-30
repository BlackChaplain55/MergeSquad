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
            return _unit.WeaponSO.GetStats(parameterType) * _unit.WeaponSO.Id;
    }
}