using UnityEditor.Experimental;
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
    private MagicSO _nullMagic;

    protected override void Awake()
    {
        StartCoroutine(HealthRegen());
        base.Awake();
        _nullMagic = Resources.Load<MagicSO>("Items/NullMagic");
        MagicSO = _nullMagic;
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

    public void SetMagic(MagicSO magic)
    {
        if (magic != null)
            MagicSO = magic;
        else
            MagicSO = _nullMagic;

        MagicStrength = _statsProvider.GetStats(UnitParameterType.MagicStrength);
        Debug.Log(_statsProvider.GetStats(UnitParameterType.MagicStrength));
        MagicRange = _statsProvider.GetStats(UnitParameterType.MagicRange);
    }

    protected override void InitDecorators()
    {
        Artifact[] artifacts;
        var artifactsRepo = GameController.Game.ArtifactsRepository;
        artifacts = artifactsRepo[UnitStats.Type];

        _statsProvider = new ArtifactUnitDecorator(_unitData, artifacts);
        _statsProvider = new UnitLevelDecorator(_statsProvider, this);
        var magic = new MagicLevelDecorator(this);
        _statsProvider = new HeroMagicItemDecorator(_statsProvider, magic);
        _unitStats = new UnitStats(_unitData);
        _unitStats.SetSnapshot(_statsProvider);
    }
}
