using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;

public class HeroPresenter : MonoBehaviour
{
    public Unit Hero { get { return _hero; } }
    [SerializeField] private UnitController unitController;
    [SerializeField] private MagicSystem magicSystem;
    [SerializeField] private MergeSystem mergeSystem;
    [SerializeField] private SpellSlot spellSlot;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Slider hpBar;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private TextMeshProUGUI playerName;
    private Unit _hero;

    private void OnValidate()
    {
        if (unitController == null)
            unitController = FindAnyObjectByType<UnitController>();
    }

    private void Start()
    {
        unitController.UnitsList.CollectionChanged += FindHero;
        GetHeroHard();

        spellSlot.OnItemOverlapChanged += mergeSystem.OverlapChanged;
    }

    public void UpdateText(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(UnitParameterType.Level):
                if (level != null) level.text = $"LVL {_hero.Level}";
                break;
            case nameof(UnitParameterType.Attack):
                if (attack != null) attack.text = _hero.UnitStats.Attack.ToString();
                break;
            case nameof(UnitParameterType.UpgradeCost):
                if (upgradeCost != null) upgradeCost.text = _hero.UnitStats.UpgradeCost.ToString();
                break;
            case nameof(UnitParameterType.Health):
                if (hpText != null)
                {
                    hpText.text = $"{Mathf.Ceil(_hero.Health)}/{Mathf.Ceil(_hero.UnitStats.MaxHealth)}";
                    hpBar.maxValue = _hero.UnitStats.MaxHealth;
                    hpBar.value = _hero.Health;
                }
                break;
            default:
                break;
        }
    }

    private void FindHero(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e == null) return;
        foreach (Unit newUnit in e.NewItems)
        {
            Debug.Log("Unit Type " + newUnit.UnitReadonlyData.Type);
            if (newUnit.UnitReadonlyData.Type == UnitType.Hero)
            {
                Debug.Log("Герой найден");
                _hero = newUnit;
                Init(_hero);
                return;
            }
        }
    }

    private void GetHeroHard()
    {
        foreach (Unit unit in unitController.UnitsList)
        {
            if (unit.UnitReadonlyData.Type == UnitType.Hero)
            {
                Debug.Log("Герой вытащен из списка юнитов");
                _hero = unit;
                Init(_hero);
                return;
            }
        }
    }

    private void Init(Unit hero)
    {
        if (hero == null) return;

        unitController.UnitsList.CollectionChanged -= FindHero;

        if (level != null) level.text = $"LVL {hero.Level}";
        if (attack != null) attack.text = hero.UnitStats.Attack.ToString();
        if (upgradeCost != null) upgradeCost.text = hero.UnitStats.UpgradeCost.ToString();
        if (hpText != null)
        {
            hpText.text = $"{Mathf.Ceil(hero.Health)}/{Mathf.Ceil(hero.UnitStats.MaxHealth)}";
            hpBar.maxValue = hero.UnitStats.MaxHealth;
            hpBar.value = hero.Health;
        }
        AddAllListeners();
    }

    private void CastMagic(ItemSO magicItem) =>
        magicSystem.CastMagic(magicItem.Type);

    private void AddAllListeners()
    {
        _hero.PropertyChanged += UpdateText;
        _hero.GetUnitStatsRef().PropertyChanged += UpdateText;
        upgradeButton.onClick.AddListener(TryUpgradeHero);
        spellSlot.OnMagicCast += CastMagic;
    }

    private void TryUpgradeHero()
    {
        if (GameController.Game.SpendSouls(_hero.UnitStats.UpgradeCost))
            _hero.Upgrade();
    }

    private void RemoveAllListeners()
    {
        _hero.PropertyChanged -= UpdateText;
        _hero.GetUnitStatsRef().PropertyChanged -= UpdateText;
        upgradeButton.onClick.RemoveListener(TryUpgradeHero);
        spellSlot.OnMagicCast -= CastMagic;
    }
}