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
    public float MagicRange
    {
        get { return _magicRange; }
        private set
        {
            _magicRange = value;
            OnPropertyChanged();
        }
    }
    private float _magicStrength;
    private float _magicRange;

    public override void Upgrade()
    {
        base.Upgrade();
        MagicStrength = _statsProvider.GetStats(UnitParameterType.MagicStrength);
    }

    public void SetMagic(MagicSO magic)
    {
        MagicSO = magic;
        MagicStrength = _statsProvider.GetStats(UnitParameterType.MagicStrength);
        MagicRange = _statsProvider.GetStats(UnitParameterType.MagicRange);
    }

    protected override void InitDecorators()
    {
        _statsProvider = new UnitLevelDecorator(_unitData, this);
        var magic = new MagicLevelDecorator(this);
        _statsProvider = new HeroMagicItemDecorator(_statsProvider, magic);
        _unitStats = new UnitStats(_unitData);
        _unitStats.SetSnapshot(_statsProvider);
    }
}
