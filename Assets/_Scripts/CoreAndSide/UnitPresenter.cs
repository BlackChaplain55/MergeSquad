using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitPresenter : MonoBehaviour
{
    public Unit Unit { get; private set; }
    public Sprite UnitSprite { get; private set; }
    public EquipmentSlot Weapon { get { return weapon; } }
    public EquipmentSlot Armor { get { return armor; } }
    public event UnityAction OnUpgradeRequest;
    public event Action<EquipmentSO> OnWeaponChanged;
    public event Action<EquipmentSO> OnArmorChanged;
    [SerializeField] private List<GameObject> elements;
    [SerializeField] private EquipmentSlot weapon;
    [SerializeField] private EquipmentSlot armor;
    [SerializeField] private Image weaponDurabilityBar;
    [SerializeField] private Image armorDurabilityBar;
    [SerializeField] private Image unitIcon;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI attack;

    private void Awake()
    {
        if (upgradeButton != null) upgradeButton.onClick.AddListener(() => OnUpgradeRequest?.Invoke());
        //OnWeaponChanged += eqs => Weapon.GetItemTransform().localPosition = Vector3.zero;
        //OnArmorChanged += eqs => Armor.GetItemTransform().localPosition = Vector3.zero;
        Activate();
    }

    public void SetUnit(Unit unit)
    {
        if (Unit != null)
            RemoveAllListeners();

        Unit = unit;
        
        if (unit == null) return;

        AddAllListeners();
        InitSet();
        weapon.ItemType = Unit.UnitReadonlyData.WeaponType;
        armor.ItemType = Unit.UnitReadonlyData.ArmorType;
    }

    public void Activate()
    {
        ToggleActive(true);

        weapon.OnItemReceived += Weapon_OnItemReceived;
        armor.OnItemReceived += Armor_OnItemReceived;
    }

    private void InitSet()
    {
        if (unitIcon != null) unitIcon.sprite = Unit.UnitReadonlyData.MainSprite;
        if (level != null) level.text = Unit.Level.ToString();
        if (attack != null) attack.text = Unit.UnitStats.Attack.ToString();
        if (health != null) health.text = Mathf.Ceil(Unit.Health).ToString();
        if (upgradeCost != null) upgradeCost.text = Unit.UnitStats.UpgradeCost.ToString();
    }

    private void AddAllListeners()
    {
        Unit.PropertyChanged += UpdateText;
        Unit.GetUnitStatsRef().PropertyChanged += UpdateText;
        OnUpgradeRequest += Unit.Upgrade;
    }

    private void RemoveAllListeners()
    {
        Unit.PropertyChanged -= UpdateText;
        Unit.GetUnitStatsRef().PropertyChanged -= UpdateText;
        OnUpgradeRequest -= Unit.Upgrade;
        weapon.OnItemReceived -= Weapon_OnItemReceived;
        armor.OnItemReceived -= Armor_OnItemReceived;
    }
    
    private void Weapon_OnItemReceived(ItemSO item)
    {
        if (item is EquipmentSO equipment)
        {
            Unit.SetWeapon(equipment);
            weapon.ResetDeathTimer(equipment);
            return;
        }
        Unit.SetWeapon(null);
    }

    private void Armor_OnItemReceived(ItemSO item)
    {
        if (item is EquipmentSO equipment)
        {
            Unit.SetArmor(equipment);
            armor.ResetDeathTimer(equipment);
            return;
        }
        Unit.SetArmor(null);
    }

    public void Clear()
    {
        ToggleActive(false);
        RemoveAllListeners();
    }

    public void UpdateText(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(UnitParameterType.Level): if (level != null) level.text = Unit.Level.ToString();
                break;
            case nameof(UnitParameterType.Attack): if (attack != null) attack.text = Unit.UnitStats.Attack.ToString();
                break;
            case nameof(UnitParameterType.Health): if (health != null) health.text = Unit.Health.ToString();
                break;
            case nameof(UnitParameterType.UpgradeCost): if (upgradeCost != null) upgradeCost.text = Unit.UnitStats.UpgradeCost.ToString();
                break;
            default:
                break;
        }
    }

    private void ToggleActive(bool flag)
    {
        if (elements == null) return;

        foreach (var el in elements)
        {
            el.SetActive(flag);
        }
    }
}
