using UnityEngine;

public class MagicLevelDecorator : ItemLevelDecorator
{
    public MagicLevelDecorator(Hero unit)
    {
        _unit = unit;
    }

    public override float GetStats(ItemParameterType parameterType)
    {
        Hero hero = _unit as Hero;

        if (hero == null)
            throw new System.NullReferenceException("Hero is null, MagicLevelDecorator");
        else if (hero.MagicSO == null)
            throw new System.NullReferenceException("MagicSO is null, MagicLevelDecorator");
        else
        {
            MagicSO magic = hero.MagicSO;
            int magicLevel = magic.Id;
            float strengthPerLevel = GameController.Game.Settings.MagicStrengthPerLevel[magic.Type];
            float rangePerLevel = GameController.Game.Settings.MagicRangePerLevel[magic.Type];

            switch (parameterType)
            {
                case ItemParameterType.MagicStrength:
                    return magic.BaseStrength + Mathf.Pow(strengthPerLevel, magicLevel);
                case ItemParameterType.MagicRange:
                    return magic.BaseRange + rangePerLevel * magicLevel;
                default:
                    return magic.GetStats(parameterType);
            }
        }
    }
}
