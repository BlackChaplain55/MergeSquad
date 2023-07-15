using UnityEngine;

public class Hero : Unit
{
    public MagicSO MagicSO { get; private set; }

    public float MagicStrength
    {
        get { return _magicStrength; }
        private set {
            _magicStrength = value;
            OnPropertyChanged();
        }
    }
    private float _magicStrength;

    protected override void InitDecorators()
    {
        _statsProvider = new UnitLevelDecorator(_unitData, this);
        var magic = new MagicLevelDecorator(this);
        _statsProvider = new HeroMagicItemDecorator(_statsProvider, magic);
        _unitStats = new UnitStats(_unitData);
        _unitStats.SetSnapshot(_statsProvider);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        MagicStrength = _statsProvider.GetStats(UnitParameterType.MagicStrength);
    }
}
