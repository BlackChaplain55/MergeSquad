using UnityEngine;
using System.Collections;

public class Hero : Unit
{
    public MagicSO MagicSO { get; private set; }
    [SerializeField] private float _healthRegen;

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
        StartCoroutine(HealthRegen());
    }

    public override void Upgrade()
    {
        base.Upgrade();
        MagicStrength = _statsProvider.GetStats(UnitParameterType.MagicStrength);
    }
    private IEnumerator HealthRegen()
    {
        var delay = new WaitForSeconds(1);
        while (State != UnitState.Die)
        {
            yield return delay;
            Health += _healthRegen;
            Health = Mathf.Clamp(Health, 0, _unitStats.MaxHealth);
        }
        yield return null;
    }
}
