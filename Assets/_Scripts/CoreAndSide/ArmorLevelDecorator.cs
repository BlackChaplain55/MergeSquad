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
            return _unit.ArmorSO.GetStats(parameterType) * _unit.ArmorSO.Id;
    }
}