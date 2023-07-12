using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;

public class HeroPresenter : MonoBehaviour
{
    public Hero Hero { get { return _hero; } }
    [SerializeField] private UnitController unitController;
    [SerializeField] private MagicSystem magicSystem;
    [SerializeField] private MergeSystem mergeSystem;
    [SerializeField] private SpellSlot spellSlot;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI magicStrength;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Slider hpBar;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private TextMeshProUGUI playerName;
    private Hero _hero;

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
            case nameof(UnitParameterType.MagicStrength):
                if (magicStrength != null) magicStrength.text = $"Magic {_hero.MagicStrength}";
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
                _hero = newUnit as Hero;
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
                if (unit is Hero)
                    Debug.Log("Герой вытащен из списка юнитов");
                _hero = unit as Hero;
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
            int currentHealth = (int)Mathf.Ceil(hero.Health);
            int maxHealth = (int)Mathf.Ceil(hero.UnitStats.MaxHealth);

            hpText.text = $"{currentHealth}/{maxHealth}";
            hpBar.maxValue = hero.UnitStats.MaxHealth;
            hpBar.value = hero.Health;
        }
        AddAllListeners();
    }

    private void CastMagic(MagicSO magicItem) =>
        magicSystem.CastMagic(_hero, magicItem);

    private void AddAllListeners()
    {
        _hero.PropertyChanged += UpdateText;
        _hero.GetUnitStatsRef().PropertyChanged += UpdateText;
        upgradeButton.onClick.AddListener(TryUpgradeHero);
        spellSlot.OnMagicCast += CastMagic;
        spellSlot.PropertyChanged += SpellSlot_PropertyChanged;
        mergeSystem.OnSlotCarryStateChanged += SetHighlight;
    }

    private void SpellSlot_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if(e.PropertyName.Equals(nameof(SpellSlot.CooldownTime)))
            TryUpdateHiglight();
    }

    private void TryUpdateHiglight()
    {
        if (spellSlot.CooldownTime <= 0)
            SetHighlight(mergeSystem.CarryingItemSlot, true);
    }

    private void SetHighlight(Slot slot, bool flag)
    {
        if (slot.CurrentItem == null) return;

        if (spellSlot.TryPlace(slot))
            spellSlot.SetHighlight(flag);
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
        mergeSystem.OnSlotCarryStateChanged -= SetHighlight;
    }
}