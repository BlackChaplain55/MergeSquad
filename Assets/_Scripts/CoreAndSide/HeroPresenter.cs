using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;

public class HeroPresenter : MonoBehaviour
{
    public Unit Hero { get { return hero; } }
    [SerializeField] private UnitController unitController;
    [SerializeField] private Unit hero;
    [SerializeField] private EquipmentSlot magicSlot;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Slider hpBar;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private TextMeshProUGUI playerName;

    private void OnValidate()
    {
        if (unitController == null)
            unitController = FindAnyObjectByType<UnitController>();
    }

    private void Start()
    {
        unitController.UnitsList.CollectionChanged += FindHero;
        GetHeroHard();
    }

    public void UpdateText(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(UnitParameterType.Level):
                if (level != null) level.text = hero.Level.ToString();
                break;
            case nameof(UnitParameterType.Attack):
                if (attack != null) attack.text = hero.UnitStats.Attack.ToString();
                break;
            case nameof(UnitParameterType.UpgradeCost):
                if (upgradeCost != null) upgradeCost.text = hero.UnitStats.UpgradeCost.ToString();
                break;
            case nameof(UnitParameterType.Health):
                if (hpText != null)
                {
                    hpText.text = $"{Mathf.Ceil(hero.Health)}/{Mathf.Ceil(hero.UnitStats.MaxHealth)}";
                    hpBar.maxValue = hero.UnitStats.MaxHealth;
                    hpBar.value = hero.Health;
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
                hero = newUnit;
                Init(hero);
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
                hero = unit;
                Init(hero);
                return;
            }
        }
    }

    private void Init(Unit hero)
    {
        if (hero == null) return;

        unitController.UnitsList.CollectionChanged -= FindHero;

        if (level != null) attack.text = hero.Level.ToString();
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

    private void AddAllListeners()
    {
        hero.PropertyChanged += UpdateText;
        hero.GetUnitStatsRef().PropertyChanged += UpdateText;
        upgradeButton.onClick.AddListener(hero.Upgrade);
    }

    private void RemoveAllListeners()
    {
        hero.PropertyChanged -= UpdateText;
        hero.GetUnitStatsRef().PropertyChanged -= UpdateText;
        upgradeButton.onClick.RemoveListener(hero.Upgrade);
    }
}